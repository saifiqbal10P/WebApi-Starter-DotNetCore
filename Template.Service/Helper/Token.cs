using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template.Core.Auth;
using Template.Core.DTO;

namespace Tempalte.Service.Helper
{
    public static class Token
    {
        public static async Task<LoginResponseDTO> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, string DeviceId, JwtIssuerOptions jwtOptions)
        {
            return new LoginResponseDTO()
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                FirstName = identity.Claims.Single(c => c.Type == "firstName").Value,
                LastName = identity.Claims.Single(c => c.Type == "lastName").Value,
                Role = identity.Claims.Single(c => c.Type == "userRole").Value,
                AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity, DeviceId),
                ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
            };
        }
    }
}
