// <copyright file="BaseEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System;

    public class BaseEntity
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
    }
}
