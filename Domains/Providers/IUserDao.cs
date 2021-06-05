// <copyright file="IUserDao.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Providers
{
    using MyAccess.Domains.Entities;

    public interface IUserDao : IRepository<UserEntity>
    {
    }
}
