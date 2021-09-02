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
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly ITestTableRepository _TestTableRepository;
        private readonly IRequestInfo<TemplateContext> _requestInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BackgroundJobService(IUnitOfWork unitOfWork
            , ITestTableRepository TestTableRepository, IRequestInfo<TemplateContext> requestInfo, IMapper mapper)
        {
            _requestInfo = requestInfo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _TestTableRepository = TestTableRepository;
        }

        public async Task TestJob()
        {
            var data = await _TestTableRepository.GetAll();
            throw new NotImplementedException();
        }
    }
}
