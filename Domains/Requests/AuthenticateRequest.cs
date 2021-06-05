// <copyright file="AuthenticateRequest.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Requests
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class AuthenticateRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "{0} is not valid.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
