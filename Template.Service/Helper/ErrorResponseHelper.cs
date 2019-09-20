using Microsoft.AspNetCore.Identity;
using Recipe.NetCore.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template.Service.Helper
{
    public static class ErrorResponseHelper
    {
        public static string GetErrorMsg(string errorMsg, IEnumerable<IdentityError> Errors)
        {
            StringBuilder errorMsgs = new StringBuilder();
            errorMsgs.Append(errorMsg);
            for (int i = 0; i < Errors.ToList().Count; i++)
            {
                errorMsgs.AppendLine(Errors.ToList()[i].Description);
            }
            return errorMsgs.ToString();
        }

        public static DataTransferObject<T> CreateErrorResponse<T>(string error)
        {
            var errorResponse = new DataTransferObject<T>();
            errorResponse.HasErrors = true;
            errorResponse.Error = new Exception(error);
            return errorResponse;
        }

        public static DataTransferObject<T> CreateErrorResponse<T>(string error , T value)
        {
            var errorResponse = new DataTransferObject<T>(value);
            errorResponse.HasErrors = true;
            errorResponse.Error = new Exception(error);
            return errorResponse;
        }
    }
}
