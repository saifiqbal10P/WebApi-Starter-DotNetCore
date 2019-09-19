using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Recipe.NetCore.Base.Interface;
using System.Threading.Tasks;

namespace Recipe.NetCore.Base.Abstract
{
    public class UnitOfWork<TDbContext> : IUnitOfWork
            where TDbContext : DbContext
    {
        private readonly IRequestInfo<TDbContext> _requestInfo;

        public UnitOfWork(IRequestInfo<TDbContext> requestInfo)
        {
            this._requestInfo = requestInfo;
        }

        public DbContext DbContext
        {
            get
            {
                return this._requestInfo.Context;
            }
        }

        public int Save()
        {
            return this._requestInfo.Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await this._requestInfo.Context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.DbContext.Database.BeginTransaction();
        }
    }
}
