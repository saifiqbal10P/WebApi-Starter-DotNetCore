using System;

namespace Recipe.NetCore.Base.Interface
{
    public interface IAuditModel<TKey> : IAuditModel, IBase<TKey>
    {
    }

    public interface IAuditModel : IBase
    {
        DateTime CreatedOn { get; set; }

        DateTime? LastModifiedOn { get; set; }

        bool IsDeleted { get; set; }

        int CreatedBy { get; set; }

        int? LastModifiedBy { get; set; }

        bool IsActive { get; set; }
    }
}
