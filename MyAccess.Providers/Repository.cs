// <copyright file="Repository.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.Contrib.Extensions;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class Repository<T> : SqlServerDataBase, IRepository<T>
        where T : BaseEntity
    {
        public bool Delete(T entity)
        {
            using var cnn = this.GetDatabaseConnection();
            entity.Active = false;
            return cnn.Update(entity);
        }

        public bool DeleteList(IEnumerable<T> entity)
        {
            using var cnn = this.GetDatabaseConnection();
            entity.ToList().ForEach(x => x.Active = false);
            return cnn.Update(entity);
        }

        public T GetById(Guid id)
        {
            using var cnn = this.GetDatabaseConnection();
            return cnn.Get<T>(id);
        }

        public IEnumerable<T> GetList()
        {
            using var cnn = this.GetDatabaseConnection();
            return cnn.GetAll<T>().Where(x => x.Active);
        }

        public int Insert(T entity)
        {
            using var cnn = this.GetDatabaseConnection();
            return (int)cnn.Insert(entity);
        }

        public int InsertList(IEnumerable<T> entity)
        {
            using var cnn = this.GetDatabaseConnection();
            return (int)cnn.Insert(entity);
        }

        public bool Update(T entity)
        {
            using var cnn = this.GetDatabaseConnection();
            return cnn.Update(entity);
        }

        public bool UpdateList(IEnumerable<T> entity)
        {
            using var cnn = this.GetDatabaseConnection();
            return cnn.Update(entity);
        }
    }
}
