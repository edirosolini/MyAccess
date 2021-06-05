// <copyright file="Repository.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper.Contrib.Extensions;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class Repository<T> : SqlServerDataBase, IRepository<T>
        where T : BaseEntity
    {
        private SqlConnection sqlConnection;

        public long Insert(T entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.Insert(entity);
        }

        public long InsertList(IEnumerable<T> entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.Insert(entity);
        }

        public T Get(Guid id)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.Get<T>(id);
        }

        public IEnumerable<T> GetList()
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.GetAll<T>().Where(x => x.Active);
        }

        public bool Update(T entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.Update(entity);
        }

        public bool UpdateList(IEnumerable<T> entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            return this.sqlConnection.Update(entity);
        }

        public bool Delete(T entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            entity.Active = false;
            return this.sqlConnection.Update(entity);
        }

        public bool DeleteList(IEnumerable<T> entity)
        {
            this.sqlConnection ??= this.GetSqlConnection();
            entity.ToList().ForEach(x => x.Active = false);
            return this.sqlConnection.Update(entity);
        }
    }
}
