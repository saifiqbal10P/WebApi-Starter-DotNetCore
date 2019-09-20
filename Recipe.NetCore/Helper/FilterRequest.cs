using Recipe.NetCore.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Common.Helper
{
    public static class FilterRequest
    {
        public static JsonapiRequest GetRequest(Dictionary<string, string> queryParams)
        {
            var jsonApiRequest = JsonapiefQueryBuilder.GetJsonApiRequest(queryParams);
            return jsonApiRequest;
        }
    }
}
