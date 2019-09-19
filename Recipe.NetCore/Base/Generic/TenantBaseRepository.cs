using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recipe.NetCore.Base.Generic;
using Recipe.NetCore.Base.Interface;

namespace Recipe.NetCore.Base.Abstract
{
    public class TenantBaseRepository<TEntity, TKey, TDbContext> : AuditableRepository<TEntity, TKey, TDbContext>
            where TEntity : class, ITenantModel<TKey>
            where TKey : IEquatable<TKey>
            where TDbContext : DbContext
    {
        public TenantBaseRepository(IRequestInfo<TDbContext> requestInfo)
            : base(requestInfo)
        {
        }

        protected override IQueryable<TEntity> DefaultListQuery => base.DefaultListQuery.Where(x =>  x.TenantId == null || x.TenantId == RequestInfo.TenantId);

        protected override IQueryable<TEntity> DefaultSingleQuery => base.DefaultSingleQuery.Where(x => x.TenantId == null || x.TenantId == RequestInfo.TenantId);

        protected override IQueryable<TEntity> DefaultUnOrderedListQuery => base.DefaultUnOrderedListQuery.Where(x => x.TenantId == null || x.TenantId == RequestInfo.TenantId);

        public override Task<TEntity> Add(TEntity entity)
        {
            entity.TenantId = (RequestInfo.TenantId > 0) ? RequestInfo.TenantId : (int?)null;
            return base.Add(entity);
        }
        
        public override Task<TEntity> Create(TEntity entity)
        {
            entity.TenantId = (RequestInfo.TenantId > 0) ? RequestInfo.TenantId : (int?)null;
            return base.Create(entity);
        }
        
    }
}
