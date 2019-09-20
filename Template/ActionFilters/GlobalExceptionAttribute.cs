using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Api.Models;

namespace Template.Api.ActionFilters
{
    public class GlobalExceptionAttribute: IExceptionFilter
    {

        private readonly ILogger _logger;
        public GlobalExceptionAttribute(ILogger<GlobalExceptionAttribute> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            int status = 500;
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var ctrlName = descriptor.ControllerName;

            string exceptionMsg = context.Exception.Message + string.Format(":  Controller: {0}  Action: {1}", ctrlName, actionName);
            var exception = new Exception(exceptionMsg, context.Exception);

            var result = new ObjectResult(new ValidationResultModel(context));
            result.StatusCode = status;
            context.Result = result;
            _logger.LogError(exception, exceptionMsg, null);

        }
    }
}
