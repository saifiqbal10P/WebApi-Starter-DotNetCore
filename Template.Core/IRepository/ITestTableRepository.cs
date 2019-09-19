using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.Core.Entity;

namespace Template.Core.IRepository
{
    public interface ITestTableRepository : IAuditableRepository<TestTable, long>
    {
        Task<TestTable> GetByName(string name); 
    }
}
