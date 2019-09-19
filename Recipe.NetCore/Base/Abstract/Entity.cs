using Recipe.NetCore.Base.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Recipe.NetCore.Base.Abstract
{
    public abstract class EntityBase<TKey> : IAuditModel<TKey>
    {
        public TKey Id { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
    }
}
