﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipe.NetCore.Base.Interface;
using Recipe.NetCore.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Template.Common.Constant.Constant;
using static Template.Common.Constant.Constant.Strings;

namespace Recipe.NetCore.Infrastructure
{
    public class RequestInfo<TDbContext> : IRequestInfo<TDbContext> where TDbContext : DbContext
    {
        private readonly IServiceScope Scope;
        private readonly IHttpContextAccessor contextAccessor;

        public RequestInfo(IServiceProvider serviceProvider, IHttpContextAccessor _contextAccessor)
        {
            contextAccessor = _contextAccessor;
            Scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        public string Role => ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.Role);

        public string UserName => ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.UserName);

        public int UserId => Convert.ToInt32(ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.Id));

        public string Email => ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.Email);

        public int? TenantId => ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.TenantId) == ""? (int?)null: Convert.ToInt32(ApplicationContext.GetHttpContextSessionItem(contextAccessor, ClaimIdentifiers.TenantId)) ; 

        public TDbContext Context => Scope.ServiceProvider.GetRequiredService<TDbContext>();


    }
}
