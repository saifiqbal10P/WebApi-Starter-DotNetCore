using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Recipe.NetCore.Base.Interface
{
#pragma warning disable S3246 // Generic type parameters should be co/contravariant when possible
    public interface IRequestInfo<TDbContext>
#pragma warning restore S3246 // Generic type parameters should be co/contravariant when possible
        where TDbContext : DbContext
    {
        int UserId { get; }

        string UserName { get; }

        int? TenantId { get; }
        string Role { get; }

        TDbContext Context { get; }
    }
}
