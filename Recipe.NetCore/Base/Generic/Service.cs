using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipe.NetCore.Base.Abstract;
using Recipe.NetCore.Base.Interface;
using Recipe.NetCore.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Common.Helper;

namespace Recipe.NetCore.Base.Generic
{
    public class Service : IService
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }

    public class Service<TRepository, TEntity, TDTO, TKey> : Service, IService<TRepository, TEntity, TDTO, TKey>
     where TEntity : IAuditModel<TKey>, new()
     where TDTO : Dto<TEntity, TKey>, new()
     where TRepository : IAuditableRepository<TEntity, TKey>
     where TKey : IEquatable<TKey>
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;

        public TRepository Repository => _repository;

        public Service(IUnitOfWork unitOfWork, TRepository repository, IMapper mapper)
            : base(unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Service(IUnitOfWork unitOfWork, TRepository repository)
            : base(unitOfWork)
        {
            _repository = repository;
        }

        protected Task<TEntity> Create(TDTO dtoObject)
        {
            TEntity entity = dtoObject.ConvertToEntity();
            return _repository.Create(entity);
        }

        public virtual async Task<DataTransferObject<TDTO>> CreateAsync(TDTO dtoObject)
        {
            var result = await Create(dtoObject);
            await UnitOfWork.SaveAsync();

            dtoObject.ConvertFromEntity(result);
            return new DataTransferObject<TDTO>(dtoObject);
        }

        public virtual async Task<IList<TDTO>> CreateAsync(IList<TDTO> dtoObjects)
        {
            List<TEntity> results = new List<TEntity>();

            foreach (TDTO dtoObject in dtoObjects)
            {
                results.Add(await Create(dtoObject));
            }

            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await UnitOfWork.SaveAsync();
            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            return Dto<TEntity, TKey>.ConvertEntityListToDtoList<TDTO>(results);
        }

        protected async Task Delete(TKey id)
        {
            await _repository.DeleteAsync(id);
        }

        protected async Task HardDelete(TKey id)
        {
            await _repository.HardDeleteAsync(id);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            await Delete(id);
            await UnitOfWork.SaveAsync();
        }

        public virtual async Task HardDeleteAsync(TKey id)
        {
            await HardDelete(id);
            await UnitOfWork.SaveAsync();
        }

        public virtual async Task DeleteAsync(IList<TKey> ids)
        {
            foreach (TKey id in ids)
            {
                await Delete(id);
            }

            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await UnitOfWork.SaveAsync();
            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public virtual async Task<int> GetCount()
        {
            return await _repository.GetCount();
        }
        
        public async Task<DataTransferObject<List<TDTO>>> GetAllPagedAsync(DataTransferObject<TEntity> model)
        {
            var results = await Repository.GetPagedResultAsync(model.Filter, model.OrderBy, model.Includes, model.Paging.PageNumber, model.Paging.PageSize);

            var collection = _mapper.Map<List<TDTO>>(results.Item2);
            model.Paging.TotalCount = results.Item1;

            return new DataTransferObject<List<TDTO>>(collection, model.Paging);
        }
        
        public virtual async Task<DataTransferObject<TDTO>> GetAsync(TKey id)
        {
            TEntity entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return null;
            }

            TDTO dto = _mapper.Map<TDTO>(entity);
            return new DataTransferObject<TDTO>(dto);
        }

        protected async Task<TEntity> Update(TDTO dtoObject)
        {
            var dbEntity = await _repository.GetAsync(dtoObject.Id);
            var entity = dtoObject.ConvertToEntity(dbEntity);
            return await _repository.Update(entity);
        }

        public virtual async Task<DataTransferObject<TDTO>> UpdateAsync(TDTO dtoObject)
        {
            var result = await Update(dtoObject);
            await UnitOfWork.SaveAsync();
            dtoObject.ConvertFromEntity(result);
            return new DataTransferObject<TDTO>(dtoObject);
        }

        public virtual async Task<IList<TDTO>> UpdateAsync(IList<TDTO> dtoObjects)
        {
            List<TEntity> results = new List<TEntity>();
            foreach (TDTO dtoObject in dtoObjects)
            {
                results.Add(await Update(dtoObject));
            }

            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await UnitOfWork.SaveAsync();
            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            return Dto<TEntity, TKey>.ConvertEntityListToDtoList<TDTO>(results);
        }

        public virtual async Task<IList<TEntity>> UpdateAsync(IList<TEntity> entityObjects)
        {
            List<Task> taskList = new List<Task>();

            foreach (var entityObject in entityObjects)
            {
                taskList.Add(_repository.Update(entityObject));
            }

            await Task.WhenAll(taskList);

            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await UnitOfWork.SaveAsync();
            UnitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            return entityObjects;
        }

        protected async Task<TEntity> SoftDelete(TDTO dtoObject)
        {
            var dbEntity = await _repository.GetAsync(dtoObject.Id);
            var entity = dtoObject.ConvertToEntity(dbEntity);
            entity.IsDeleted = true;
            return await _repository.Update(entity);
        }

        public async Task BulkDelete(IList<TKey> ids)
        {
            foreach (TKey id in ids)
            {
                await Delete(id);
            }

            await UnitOfWork.SaveAsync();
        }

        public async Task BulkHardDelete(IList<TKey> ids)
        {
            foreach (TKey id in ids)
            {
                await HardDelete(id);
            }

            await UnitOfWork.SaveAsync();
        }

        protected virtual async Task SaveContext()
        {
            await UnitOfWork.SaveAsync();
        }
    }
}
