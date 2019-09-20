using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Template.Core.DTO
{
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string DeviceId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public bool IsWebUser { get; set; }

    }
}
