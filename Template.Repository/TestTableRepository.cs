using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using Template.Core.DbContext;
using Template.Core.Entity;
using Template.Core.IRepository;

namespace Template.Repositor
{
    public class TestTableRepository : AuditableRepository<TestTable, long, TemplateContext>, ITestTableRepository
    {
        public TestTableRepository(IRequestInfo<TemplateContext> requestInfo)
         : base(requestInfo)
        {
        }

        public Task<TestTable> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
