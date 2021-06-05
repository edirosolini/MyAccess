// <copyright file="IBaseService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    using System;
    using MyAccess.Domains.Models;

    public interface IBaseService<T>
        where T : class
    {
        ListModel<T> Get(int since, int limit);

        T Get(Guid id);

        ResponseModel Delete(Guid id);

        ResponseModel Insert(T entity);

        ResponseModel Update(T entity);
    }
}
