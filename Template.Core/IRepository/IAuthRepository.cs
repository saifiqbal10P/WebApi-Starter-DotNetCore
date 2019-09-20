using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Core.Entity;

namespace Template.Core.IRepository
{
    public interface IAuthRepository: IAuditableRepository<AuthStore, long>
    {
    }
}
