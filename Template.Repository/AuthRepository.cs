using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Core.DbContext;
using Template.Core.Entity;
using Template.Core.IRepository;

namespace Template.Repository
{
    public class AuthRepository : AuditableRepository<AuthStore, long, TemplateContext>, IAuthRepository
    {
        public AuthRepository(IRequestInfo<TemplateContext> requestInfo)
         : base(requestInfo)
        {
        }
    }
}
