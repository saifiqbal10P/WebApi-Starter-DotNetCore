using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Core.Entity
{
    public class AuthStore : SetupEntity
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

        public string FirebaseToken { get; set; }

        public string UserName { get; set; }

        public bool IsRevoked { get; set; }

        public string DeviceId { get; set; }
    }
}
