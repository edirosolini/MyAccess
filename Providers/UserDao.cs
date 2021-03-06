﻿// <copyright file="UserDao.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class UserDao : Repository<UserEntity>, IUserDao
    {
    }
}
