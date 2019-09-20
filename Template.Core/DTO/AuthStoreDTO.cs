using Recipe.NetCore.Base.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Template.Core.Entity;

namespace Template.Core.DTO
{
    public class AuthStoreDTO : Dto<AuthStore, long>
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string DeviceId { get; set; }

        public bool IsWebUser { get; set; }
    }

}
