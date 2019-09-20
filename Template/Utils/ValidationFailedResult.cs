using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Api.Models;

namespace Template.Utils
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public ValidationFailedResult(Exception ex)
        : base(new ValidationResultModel(ex))
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
