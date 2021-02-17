// <copyright file="IRepository.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Providers
{
    using System;
    using System.Collections.Generic;
    using MyAccess.Domains.Entities;

    public interface IRepository<T>
        where T : BaseEntity
    {
        bool Delete(T entity);

        bool DeleteList(IEnumerable<T> entity);

        bool Update(T entity);

        bool UpdateList(IEnumerable<T> entity);

        IEnumerable<T> GetList();

        int Insert(T entity);

        int InsertList(IEnumerable<T> entity);

        T GetById(Guid id);
    }
}
