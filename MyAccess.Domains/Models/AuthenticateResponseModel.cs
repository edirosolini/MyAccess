// <copyright file="AuthenticateResponseModel.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Models
{
    using System;
    using MyAccess.Domains.Entities;

    public class AuthenticateResponseModel
    {
        public Guid Id { get; set; }

        public string BusinessName { get; set; }

        public string TokenAccess { get; set; }

        public string TokenType { get; } = "Bearer";

        public AuthenticateResponseModel(UserEntity user, string token)
        {
            this.Id = user.Id;
            this.BusinessName = $"{user.LastName} {user.FirstName}";
            this.TokenAccess = token;
        }
    }
}
