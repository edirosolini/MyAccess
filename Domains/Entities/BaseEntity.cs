// <copyright file="BaseEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dapper.Contrib.Extensions;

    public class BaseEntity
    {
        [Column(Order = 0)]
        [ExplicitKey]
        [Required(ErrorMessage = "{0} is required")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(Order = 1)]
        [Required(ErrorMessage = "{0} is required")]
        public bool Active { get; set; } = true;
    }
}
