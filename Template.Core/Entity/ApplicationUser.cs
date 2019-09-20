using Microsoft.AspNetCore.Identity;
using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Template.Core.Entity
{
    public class ApplicationUser : IdentityUser<long>, IAuditModel<long>
    {
        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(100)]
        public string DeviceId { get; set; }

        public int UserType { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool IsActive { get; set; }
    }
}
