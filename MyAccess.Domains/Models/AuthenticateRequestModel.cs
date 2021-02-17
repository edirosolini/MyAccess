// <copyright file="AuthenticateRequestModel.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AuthenticateRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
