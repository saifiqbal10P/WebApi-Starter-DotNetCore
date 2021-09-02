using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipe.NetCore.Base.Interface;
using Template.Core.DbContext;
using Template.Core.IService;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using Recipe.NetCore.Base.Abstract;
using Template.Core.DTO;
using Microsoft.Extensions.Logging;
using Template.ActionFilters;
using Hangfire;

namespace Template.Controllers
{
    [Produces("application/json")]
    [Route("api/Tests")]
    [Authorize]
    public class TestController : BaseController
    {

        protected readonly ITestTableService testService;
        protected readonly IRequestInfo<TemplateContext> requestInfo;
        protected readonly ILogger<TestController> _logger;
        protected readonly IBackgroundJobService _backgroundJobService;
        public TestController(ITestTableService _testService, IRequestInfo<TemplateContext> _requestinfo, ILogger<TestController> logger,IBackgroundJobService backgroundJobService)
        {
            this.testService = _testService;
            this.requestInfo = _requestinfo;
            this._logger = logger;
            _backgroundJobService = backgroundJobService;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(DataTransferObject<List<TestTableDTO>>))]
        [ValidateModel]
        [Route("Results")]
        public async Task<IActionResult> GetAllTestResults()
        {

            _logger.LogInformation("Get All Results Called...");  // This is just an example. logger can be injected to service layer as well..
            return this.JsonResponse(await testService.GetAll());
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(DataTransferObject<List<TestTableDTO>>))]
        [ValidateModel]
        [Route("ExecuteBackgroundJob")]
        public async Task<IActionResult> Execute()
        {
            _logger.LogInformation("Get All Results Called...");  // This is just an example. logger can be injected to service layer as well..
            Hangfire.BackgroundJob.Enqueue(()=>this.ProcedureExport());
            return this.JsonResponse("ok");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AutomaticRetry(Attempts = 0)]
        public async Task ProcedureExport()  //Just for testing purpose. Move to some queueHelper to support multiple server queues
        {
            await _backgroundJobService.TestJob();
        }
    }
}