using Recipe.NetCore.Base.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Template.Core.Entity
{
    public class SetupEntity : EntityBase<long>
    {
        [Required]
        [MaxLength(100)]
        public virtual string Name { get; set; }
    }
}
