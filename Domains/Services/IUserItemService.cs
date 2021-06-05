// <copyright file="IUserItemService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    using System;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;

    public interface IUserItemService : IBaseService<UserItemEntity>
    {
        ListModel<UserItemEntity> GetByUserId(Guid userId);
    }
}
