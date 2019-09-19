using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Api.Models;

namespace Template.Api.ActionFilters
{
    public class GlobalExceptionAttribute: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int status = 500;
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var ctrlName = descriptor.ControllerName;

            var exception = new Exception(context.Exception.Message + string.Format(":  Controller: {0}  Action: {1}", ctrlName, actionName), context.Exception);

            var result = new ObjectResult(new ValidationResultModel(context));
            result.StatusCode = status;
            context.Result = result;

        }
    }
}
