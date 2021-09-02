using AutoMapper;
using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;

        public TestTableService(IUnitOfWork unitOfWork
            , ITestTableRepository repository, IRequestInfo<TemplateContext> requestInfo,IMapper mapper)
            : base(unitOfWork, repository)
        {
            _requestInfo = requestInfo;
            _mapper = mapper;
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

        public Task<DataTransferObject<List<TestTableDTO>>> GetByName(DataTransferObject<TestTableDTO> model)
        {
            throw new NotImplementedException();
        }
        public async Task<DataTransferObject<List<TestTableDTO>>> GetAll()
        {
            await this.Repository.Create(new TestTable() { Name = "Saif", Classification = "test" });
            _requestInfo.Context.SaveChanges();
            var result=await this.Repository.GetAll();
            var response= _mapper.Map<List<TestTable>, List<TestTableDTO>>(result.ToList());
            return new DataTransferObject<List<TestTableDTO>>(response);
        }
    }
}
