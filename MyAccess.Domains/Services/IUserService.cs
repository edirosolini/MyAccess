// <copyright file="IUserService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    using System;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;

    public interface IUserService : IBaseService<UserEntity>
    {
        AuthenticateResponseModel Authenticate(AuthenticateRequestModel model);

        ResponseModel ChangePassword(Guid id, ChangePasswordModel model);
    }
}
