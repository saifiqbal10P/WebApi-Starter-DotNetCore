﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Recipe.NetCore.Base.Interface
{
    public interface IUnitOfWork
    {
        DbContext DbContext { get; }

        Task<int> SaveAsync();

        int Save();

        IDbContextTransaction BeginTransaction();
    }
}
