using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Common.Constant
{
    public static class Constant
    {
        public static class Strings
        {
            public static Dictionary<string, List<string>> FileTypes
            {
                get
                {
                    Dictionary<string, List<string>> retObj = new Dictionary<string, List<string>>();

                    retObj.Add("Document", new List<string>()
                    {
                        "jpg",
                        "jpeg",
                        "png",
                        "gif",
                        "doc",
                        "pdf",
                        "ppt",
                        "xls",
                        "docx",
                        "pptx",
                        "xlsx",
                        "xml",
                        "txt"
                    });

                    retObj.Add("Video", new List<string>()
                    {
                        "wmv",
                        "mp4",
                        "m4a",
                        "m4v"
                    });

                    retObj.Add("Audio", new List<string>()
                    {
                        "mp3"
                    });

                    return retObj;
                }
            }

            public static class ClaimIdentifiers
            {
                public const string Role = "role";
                public const string Id = "id";
                public const string Email = "userEmail";
                public const string UserRoles = "userRole";
                public const string DeviceID = "deviceId";
                public const string FirstName = "firstName";
                public const string LastName = "lastName";
                public const string PracticeId = "practiceId";
                public const string UserName = "userName";
                public const string TenantId = "tenantId";
            }

            public static class Claims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}
