// <copyright file="IUserService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    using System;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;

    public interface IUserService : IBaseService<UserEntity>
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        ResponseModel ChangePassword(Guid id, ChangePasswordModel model);

        ResponseModel ForgotPassword(string username, string authorization);

        AuthenticateResponse GetByUsername(string username);
    }
}
