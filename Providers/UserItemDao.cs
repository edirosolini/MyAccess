// <copyright file="UserItemDao.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class UserItemDao : Repository<UserItemEntity>, IUserItemDao
    {
    }
}
