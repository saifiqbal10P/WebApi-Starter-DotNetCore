using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tempalte.Service.Helper;
using Template.Common.Configuration;
using Template.Common.Helper;
using Template.Core.Auth;
using Template.Core.DbContext;
using Template.Core.DTO;
using Template.Core.Entity;
using Template.Core.IRepository;
using Template.Core.IService;
using Template.Service.Helper;
using static Template.Common.Constant.Constant;

namespace Template.Service
{
    public class AuthService : Service<IAuthRepository, AuthStore, AuthStoreDTO, long>, IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly UserManager<ApplicationUser> userIdentityManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly RefreshTokenConfiguration _refreshTokenConfig;
        private readonly IUnitOfWork unitOfWork;

        private readonly IRequestInfo<TemplateContext> requestInfo;

        private readonly TemplateContext dbContext;

        public AuthService(
            IUnitOfWork _unitOfWork,
            IAuthRepository repository,
            UserManager<ApplicationUser> _userManager,
            IJwtFactory jwtFactory,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<RefreshTokenConfiguration> refreshTokenConfig,
            IMapper mapper,
            IRequestInfo<TemplateContext> _requestInfo, TemplateContext _dbContext
            ) : base(_unitOfWork, repository)
        {
            _repository = repository;
            _mapper = mapper;
            userIdentityManager = _userManager;
            _jwtFactory = jwtFactory;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;
            _refreshTokenConfig = refreshTokenConfig.Value;
            unitOfWork = _unitOfWork;
            requestInfo = _requestInfo;
            dbContext = _dbContext;
        }

        public async Task<DataTransferObject<AuthStore>> InsertAsync(AuthStore tokenObject)
        {
            await _repository.Create(tokenObject);
            return new DataTransferObject<AuthStore>(tokenObject);
        }

        public async Task<DataTransferObject<LoginResponseDTO>> Login(LoginDTO credentials)
        {
            var response = new DataTransferObject<LoginResponseDTO>();
            var appUser = _mapper.Map<ApplicationUser>(credentials);
            var error = await ValidateUser(credentials.UserName, credentials.Password);
            if (error != "")
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(error);
            }
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password, credentials.DeviceId);
            if (identity == null)
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(ErrorStrings.LoginFailed);
            }
            var jwt = await Token.GenerateJwt(identity, _jwtFactory, credentials.UserName, credentials.DeviceId, _jwtOptions);
            var refreshToken = GetRefreshToken(appUser);
            var refreshTokenExpiry = _refreshTokenConfig.RefreshTokenExpiry;
            jwt.RefreshToken = refreshToken;

            var prevToken = await _repository.Find(e => e.UserName == appUser.UserName && e.DeviceId == appUser.DeviceId);
            if (prevToken == null)
            {
                await this.Repository.Create(new AuthStore() { Token = jwt.AuthToken, RefreshToken = refreshToken, RefreshTokenExpiry = refreshTokenExpiry, UserName = credentials.UserName, DeviceId = credentials.DeviceId, IsRevoked = false, Name = credentials.UserName });
            }
            else
            {
                prevToken.Token = jwt.AuthToken;
                prevToken.RefreshToken = refreshToken;
                prevToken.IsRevoked = false;
                prevToken.RefreshTokenExpiry = refreshTokenExpiry;
                _mapper.Map<LoginDTO>(prevToken);
                await this.Repository.Update(prevToken);
            }
            await SaveContext();

            return new DataTransferObject<LoginResponseDTO>(jwt);
        }

        #region ValidateUser
        private async Task<string> ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult<string>(ErrorStrings.LoginFailed);
            }

            var userToVerify = await userIdentityManager.FindByNameAsync(userName);
            if (userToVerify == null)
            {
                return await Task.FromResult<string>(ErrorStrings.LoginFailed);
            }

            if (userToVerify.IsDeleted)
            {
                return await Task.FromResult<string>(ErrorStrings.UserDeleted);
            }

            if (!userToVerify.IsActive)
            {
                return await Task.FromResult<string>(ErrorStrings.UserInactive);
            }

            if (userToVerify.AccessFailedCount == 3)
            {
                return await Task.FromResult<string>(ErrorStrings.UserLocked);
            }
            return await Task.FromResult<string>("");
        }
        #endregion

        #region GetClaimsIdentity
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password, string deviceID)
        {
            var userToVerify = await userIdentityManager.FindByNameAsync(userName);
            if (await userIdentityManager.CheckPasswordAsync(userToVerify, password))
            {
                var role = (await userIdentityManager.GetRolesAsync(userToVerify)).FirstOrDefault();
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify, deviceID, role, userName));
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string deviceId)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            var userToVerify = await userIdentityManager.FindByNameAsync(userName);
            if (userToVerify != null)
            {
                var role = (await userIdentityManager.GetRolesAsync(userToVerify)).FirstOrDefault();
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify, deviceId, role, userName));
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }
        #endregion

        #region RefreshToken
        private string GetRefreshToken(ApplicationUser appUser)
        {
            return _passwordHasher.HashPassword(appUser, Guid.NewGuid().ToString()).Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);
        }

        public async Task<DataTransferObject<LoginResponseDTO>> RefreshToken(AuthStoreDTO tokenDto)
        {
            var response = new DataTransferObject<LoginResponseDTO>();
            var queryParams = new Dictionary<string, string>();
            queryParams.Add("filter.RefreshToken", tokenDto.RefreshToken);
            var jsonApiRequest = FilterRequest.GetRequest(queryParams);
            var existingToken = (await _repository.GetAll(jsonApiRequest)).FirstOrDefault();
            if (existingToken == null)
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(ErrorStrings.InvalidRefreshToken);
            }
            else if (existingToken.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(ErrorStrings.RefreshTokenExpired);
            }
            else if (existingToken.IsRevoked)
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(ErrorStrings.TokenAlreadyRevoked);
            }
            var identity = await GetClaimsIdentity(existingToken.UserName, existingToken.DeviceId);
            if (identity == null)
            {
                return ErrorResponseHelper.CreateErrorResponse<LoginResponseDTO>(ErrorStrings.InvalidRefreshToken);
            }
            var jwt = await Token.GenerateJwt(identity, _jwtFactory, existingToken.UserName, tokenDto.DeviceId, _jwtOptions);
            var appUser = await userIdentityManager.FindByNameAsync(existingToken.UserName);
            var refreshToken = GetRefreshToken(appUser);
            var refreshTokenExpiry = _refreshTokenConfig.RefreshTokenExpiry;
            jwt.RefreshToken = refreshToken;

            await this.Repository.Create(new AuthStore() { Token = jwt.AuthToken, RefreshToken = refreshToken, RefreshTokenExpiry = refreshTokenExpiry, UserName = existingToken.UserName, DeviceId = tokenDto.DeviceId, IsRevoked = false });
            await DeleteAsync(existingToken.Id);
            await SaveContext();
            return new DataTransferObject<LoginResponseDTO>(jwt);
        }

        public Task<DataTransferObject<RevokeResponseDTO>> RevokeToken(AuthStoreDTO tokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<DataTransferObject<bool>> Logout()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}