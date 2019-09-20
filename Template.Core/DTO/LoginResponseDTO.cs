using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Core.DTO
{
    public class LoginResponseDTO
    {
        public string Id { get; set; }

        public string AuthToken { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }

        public string FirebaseToken { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string Picture { get; set; }

        public string PracticeLogo { get; set; }

        public string PracticeName { get; set; }

        public bool OnBoardingCompleted { get; set; }

        public DateTime? OnBoardingDate { get; set; }
    }
}
