using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Template.Api;
using Template.Api.ActionFilters;
using Template.Core.DbContext;
using Template.Core.Entity;

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
            //AddJWT(services);
            //services.AddAutoMapper(typeof(Startup));
            //AddSwagger(services);
            //AddAuthorization(services);
            //AddHangFire(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        private void AddDependencies(IServiceCollection services)
        {
            var dependencyInjection = new DependencyInjection();
            dependencyInjection.Map(services, this.Configuration);
        }

        private void AddMVC(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionAttribute));
            });
        }
        private void AddEntityFramework(IServiceCollection services)
        {
            services.AddDbContext<TemplateContext>(options =>
                                 options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

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
    }
}
