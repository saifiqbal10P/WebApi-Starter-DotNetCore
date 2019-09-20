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

        public static class ErrorStrings
        {
            //jwt
            public const string NonZeroTimeSpan = "Must be a non-zero TimeSpan.";


            public const string InvalidBase64 = "Base64 string is invalid";
            public const string NotFound = "{0} not found.";
            public const string LoginFailed = "Please enter valid username / password";
            public const string MissMatchPass = "Password and confirm password does not match";
            public const string IncorrectPassMsg = "Incorrect current password";
            public const string ObjectReferenceNotFound = "Object not found";
            public const string EmailAddressNotValid = "Email is not valid";
            public const string ErrorSendingEmail = "Error Sending Email";
            public const string FileNotFound = "File not Found at ";
            public const string InvalidImportFile = "Import file is invalid.";
            public const string UniqueIdentifierAlreadyExists = "Unique Identifier already exists";

            //User
            public const string UserNotFound = "User not found.";
            public const string UserLocked = "User is locked. Please contact Administrator.";
            public const string UserRegisterFailed = "Unable to register user account.";
            public const string UserUpdateFailed = "Unable to update user account.";
            public const string UserDeleteFailed = "Unable to delete user account.";
            public const string UserDeleted = "User doesn't exist";
            public const string UserInactive = "User is inactive.";
            public const string EmailAlreadyExists = "A user with this email address is already registered in the system, please use any other email address and try again.";
            public const string InvalidEmailOrInvitationCode = "Invalid email or invitation code.";
            public const string UserNotFoundDueToInvalidEmail = "User not found. Please enter email address on which the invitation code is received.";
            public const string Invalid = "{0} invalid.";


            //Role
            public const string RoleNotFound = "Role not found.";
            public const string UnAuthorizedUser = "User is not Authorized";

            //Token
            public const string InvalidRefreshToken = "Invalid refresh token provided.";
            public const string InvalidResetPassToken = "Password reset token has expired or has already been used.";
            public const string TokenAlreadyRevoked = "Token has already been revoked.";
            public const string RefreshTokenExpired = "Session Expired.";

            //Picture
            public const string InvalidPictureFormat = "Image format is not valid.";

            //SetupData
            public const string SetupDataNotFound = "File missing.";

            //MediaLibrary
            public const string FileMissing = "Setup Data not found.";
            public const string FileFormat = "File Format is not allowed.";
            public const string TagsNotFound = "Tags not found.";
            public const string FileSizeExceeded = "The file size has exceeded the maximum limit of 20 Mb.";

            //Appointment
            public const string InvalidStartEndDateCompare = "Start Time Can't be greater then End time.";
            public const string InvalidBackDate = "Can't select back date.";
            public const string AppointmentAlreadyExists = "Appointment already exists, Please select another time.";
            public const string InvalidAppointmentDuration = "Appointment duration can't exceed 1 hour.";

            public const string EmptyMessage = "Empty message is not allowed.";
            public const string MessageSendHimself = "Message send to himself is not allowed.";

            //Invalid Password
            public const string InvalidPassWord = "Password should contain atlease one uppercase, lowercase, numeric, special character and it must be atleast of 8 characters.";

            //User Update
            public const string FirstName = "First Name is required.";
            public const string LastName = "Last Name is required.";
            public const string FirstNameMax = "Maximum 255 characters are required in First Name.";
            public const string LastNameMax = "Maximum 255 characters are required in Last Name.";
            public const string DateOfBirth = "Invalid Date of birth.";
            public const string Gender = "Invalid Gender.";
            public const string StateMax = "Maximum 20 characters are required in State.";
            public const string ZipMax = "Maximum 20 characters are required in Zip.";
            public const string StateMin = "Minimum 3 characters are required in State.";
            public const string AddressMax = "Maximum 500 characters are required in Address.";
            public const string EmailMax = "Maximum 256 characters are required in Email.";
            public const string FirstNameLength = "Minimum 3 and Maximum 50 characters are allowed in First Name.";
            public const string LastNameLength = "Minimum 3 and Maximum 50 characters are allowed in Last Name.";
            public const string PhoneLength = "Minimum 7 and Maximum 15 characters are allowed in Phone.";


            //UnitSystem
            public const string UnitSystemNotFound = "Unit System is required.";



            public const string NpiNotFound = "Incorrect NPI";


        }
    }
}
