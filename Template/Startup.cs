using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Template.Api;
using Template.Api.ActionFilters;
using Template.Core.Auth;
using Template.Core.DbContext;
using Template.Core.Entity;
using Template.Core.Migrations;

namespace Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddDependencies(services);
            AddMVC(services);
            AddEntityFramework(services);
            AddSwagger(services);
            AddJWT(services);
            AddMVC(services);
            services.AddAutoMapper(typeof(Startup));
            AddAuthorization(services);
            AddHangfire(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Configurations dbMigrationsConfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API V1");
            });

            try
            {
                dbMigrationsConfig.SeedData().Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        private void AddDependencies(IServiceCollection services)
        {
            var dependencyInjection = new DependencyInjection();
            dependencyInjection.Map(services, this.Configuration);
        }

        private void AddMVC(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(GlobalExceptionAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        private void AddSwagger(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });

            services.AddSwaggerGenNewtonsoftSupport();  //To use newtonsoft json serializer as default.
        }

        private void AddJWT(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            var SecretKey = Configuration.GetSection("TokenSecretKey").Value;

            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                    options.ValidFor = TimeSpan.FromMinutes(Convert.ToDouble(jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidFor)]));
                });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(configureOptions =>
                    {
                        configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                        configureOptions.TokenValidationParameters = tokenValidationParameters;
                        configureOptions.SaveToken = true;
                    });
        }

        private void AddEntityFramework(IServiceCollection services)
        {
            services.AddDbContext<TemplateContext>(options =>
                                 options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<TemplateContext>()
             .AddDefaultTokenProviders();

            var builder = services.AddIdentityCore<ApplicationUser>(x =>
            {
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = true;
                x.Password.RequireNonAlphanumeric = true;
                x.Password.RequiredLength = 8;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);
            builder.AddEntityFrameworkStores<TemplateContext>().AddDefaultTokenProviders();
        }

        private void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();
        }

        private void AddHangfire(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfireServer();
        }
    }
}
