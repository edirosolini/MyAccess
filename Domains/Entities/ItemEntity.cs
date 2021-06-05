// <copyright file="ItemEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Items")]
    public class ItemEntity : BaseEntity
    {
        [Required(ErrorMessage = "{0} is required")]
        public Guid SystemId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public Guid TypeId { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public string Key
        {
            get { return $"{this.System?.Name?.Replace(' ', '_')}-{this.Type?.Name?.Replace(' ', '_')}-{this.Name?.Replace(' ', '_')}"; }
        }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 50 characters.")]
        [DataType(DataType.Text)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be minimum 3 characters and a maximum of 50 characters.")]
        [DataType(DataType.Text)]
        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Order")]
        public int? Order { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public SystemEntity System { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public TypeEntity Type { get; set; }
    }
}
