﻿using System.Collections.Generic;

namespace EasyAccess.Infrastructure.Repository
{
    interface IRepositoryBase<TEntity> : IRepository where TEntity : class
    {
        IEnumerable<TEntity> LoadAll();
        TEntity FindById(params object[] id);
        TEntity this[params object[] id] { get; }
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(params object[] id);
    }
}
