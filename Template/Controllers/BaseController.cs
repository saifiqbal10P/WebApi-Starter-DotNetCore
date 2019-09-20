using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipe.NetCore.Base.Abstract;
using Template.Common.Helper;
using Template.Utils;

namespace Template.Controllers
{
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected virtual dynamic ThrowValidationError(Exception ex)
        {
            return new ValidationFailedResult(ex);
        }

        protected virtual dynamic JsonResponse(dynamic obj)
        {
            if (obj.HasErrors)
            {
                return ThrowValidationError(obj.Error);
            }
            else
            {
                return this.Ok(obj);
            }
        }

        protected virtual dynamic JsonResponse<TDTO, TEntity>(DataTransferObject<TEntity> entityObject)
            where TDTO : new()
        {
            if (entityObject.HasErrors)
            {
                return new ValidationFailedResult(entityObject.Error);
            }
            else
            {
                ObjectHelper.CopyObject<Paging>(entityObject.Paging);

                DataTransferObject<TDTO> dtoObject = new DataTransferObject<TDTO>();

                TDTO instance = new TDTO();
                List<Expression<Func<TDTO, object>>> includes = new List<Expression<Func<TDTO, object>>>();

                ObjectHelper.CopyObject(entityObject.Result, ref instance);

                dtoObject.Paging = entityObject.Paging;
                dtoObject.Result = instance;

                ObjectHelper.CopyObject(entityObject.Includes, ref includes);
                dtoObject.Includes = includes;

                return this.Ok(dtoObject);
            }
        }

    }
}