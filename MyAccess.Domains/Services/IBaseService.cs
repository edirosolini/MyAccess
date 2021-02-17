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
        object GetList(int since, int limit);

        object GetById(Guid id);

        ResponseModel Delete(Guid id);

        ResponseModel Insert(T entity);

        ResponseModel Update(T entity);
    }
}
