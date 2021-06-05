// <copyright file="UserItemEntity.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UsersItems")]
    public class UserItemEntity : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid ItemId { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public UserEntity User { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public ItemEntity Item { get; set; }
    }
}
