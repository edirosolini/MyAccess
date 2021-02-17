// <copyright file="UserEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 50 characters.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 50 characters.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 16 characters.")]
        [JsonIgnore]
        public string Password { get; set; }
    }
}
