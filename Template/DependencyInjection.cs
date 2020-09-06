using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Common.Configuration;
using Template.Core.Auth;
using Template.Core.DbContext;
using Template.Core.Entity;
using Template.Core.IRepository;
using Template.Core.IService;
using Template.Repositor;
using Template.Repository;
using Template.Service;


namespace Template.Api
{
    public class DependencyInjection
    {
        public void Map(IServiceCollection services, IConfiguration Configuration)
        {
            #region Configurations

            services.AddScoped<Core.Migrations.Configurations>();
            services.Configure<RefreshTokenConfiguration>(Configuration.GetSection("RefreshToken"));

            #endregion 

            #region Base
            services.AddHttpContextAccessor();
            services.AddScoped<IRequestInfo<TemplateContext>, Recipe.NetCore.Infrastructure.RequestInfo<TemplateContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TemplateContext>>();
            services.AddScoped(typeof(IService), typeof(Recipe.NetCore.Base.Generic.Service));
            #endregion

            #region Test
            services.AddScoped(typeof(ITestTableRepository), typeof(TestTableRepository));
            services.AddScoped(typeof(ITestTableService), typeof(TestTableService));
            #endregion

            #region Auth
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));
            services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            #endregion    
        }
    }
}
