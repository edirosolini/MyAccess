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
        long Insert(T entity);

        long InsertList(IEnumerable<T> entity);

        T Get(Guid id);

        IEnumerable<T> GetList();

        bool Update(T entity);

        bool UpdateList(IEnumerable<T> entity);

        bool Delete(T entity);

        bool DeleteList(IEnumerable<T> entity);
    }
}
