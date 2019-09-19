using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.Core.DbContext;
using Template.Core.DTO;
using Template.Core.Entity;
using Template.Core.IRepository;
using Template.Core.IService;

namespace Template.Service
{
    public class TestTableService : Service<ITestTableRepository, TestTable, TestTableDTO, long>, ITestTableService
    {
        private readonly ITestTableRepository TestTableRepository;
        private readonly IRequestInfo<TemplateContext> _requestInfo;

        public TestTableService(IUnitOfWork unitOfWork
            , ITestTableRepository repository, IRequestInfo<TemplateContext> requestInfo)
            : base(unitOfWork, repository)
        {
            _requestInfo = requestInfo;
        }



        public Task BulkDelete(IList<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IList<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task<DataTransferObject<TestTableDTO>> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTransferObject<List<TestTableDTO>>> GetByName(DataTransferObject<TestTable> model)
        {
            throw new NotImplementedException();
        }
    }
}
