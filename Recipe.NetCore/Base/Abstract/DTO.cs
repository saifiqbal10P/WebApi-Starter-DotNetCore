using Recipe.NetCore.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Common.Helper;

namespace Recipe.NetCore.Base.Abstract
{
    public class Dto<TEntity, TKey> : IBase<TKey>
          where TEntity : IAuditModel<TKey>, new()
    {

        public TKey Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; } = true;

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public Dto()
        {

        }

        public Dto(TEntity entity)
        {
            this.Id = entity.Id;
        }

        public TEntity ConvertToEntity()
        {
            TEntity entity = new TEntity();
            return this.ConvertToEntity(entity);
        }

        public virtual TEntity ConvertToEntity(TEntity entity)
        {
            entity.Id = this.Id == null || this.Id.Equals(default(TKey)) ? entity.Id : this.Id;
            return entity;
        }

        public virtual void ConvertFromEntity(TEntity entity)
        {
            this.Id = entity.Id;
        }

        public static List<TDTO> ConvertEntityListToDtoList<TDTO>(IEnumerable<TEntity> entityList) where TDTO : Dto<TEntity, TKey>, new()
        {
            var result = new List<TDTO>();

            if (entityList != null)
            {
                foreach (var entity in entityList)
                {
                    var dto = new TDTO();
                    dto.ConvertFromEntity(entity);
                    result.Add(dto);
                }
            }

            return result;
        }

        public static IList<TEntity> ConvertDtoListToEntity(IEnumerable<Dto<TEntity, TKey>> dtoList, IEnumerable<TEntity> entityList)
        {
            var result = new List<TEntity>();

            if (dtoList != null)
            {
                foreach (var dto in dtoList)
                {
                    var entityFromDb = entityList.SingleOrDefault(x => x.Id.Equals(dto.Id));
                    if (entityFromDb != null)
                    {
                        result.Add(dto.ConvertToEntity(entityFromDb));
                    }
                    else
                    {
                        result.Add(dto.ConvertToEntity());
                    }
                }
            }

            foreach (var entity in entityList.Where(x => !dtoList.Any(y => y.Id.Equals(x.Id))))
            {
                entity.IsDeleted = true;
                result.Add(entity);
            }
            return result;
        }

        public static IList<TEntity> ConvertDtoListToEntity(IEnumerable<Dto<TEntity, TKey>> dtoList)
        {
            var result = new List<TEntity>();
            if (dtoList != null)
            {
                foreach (var dto in dtoList)
                {
                    result.Add(dto.ConvertToEntity());
                }
            }
            return result;
        }

        public T CopyTo<T>() where T : new()
        {
            return ObjectHelper.CopyObject<T>(this);
        }
    }
}
