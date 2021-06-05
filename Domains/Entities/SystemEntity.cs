// <copyright file="SystemEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Systems")]
    public class SystemEntity : BaseEntity
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 50 characters.")]
        [DataType(DataType.Text)]
        [DisplayName("Name")]
        public string Name { get; set; }

        public IEnumerable<ItemEntity> Items { get; set; }
    }
}
