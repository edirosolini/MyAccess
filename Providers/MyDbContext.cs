// <copyright file="MyDbContext.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using Microsoft.EntityFrameworkCore;
    using MyAccess.Domains.Entities;

    public class MyDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SystemEntity> Systems { get; set; }

        public DbSet<TypeEntity> Types { get; set; }

        public DbSet<ItemEntity> Items { get; set; }

        public DbSet<UserItemEntity> UsersItems { get; set; }

        public MyDbContext()
            : base()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> option)
            : base(option)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>()
                .HasIndex(u => u.EmailAddress)
                .IsUnique();

            builder.Entity<SystemEntity>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<TypeEntity>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<ItemEntity>()
                .HasIndex(u => new { u.SystemId, u.TypeId, u.Name })
                .IsUnique();

            builder.Entity<UserItemEntity>()
                .HasIndex(u => new { u.UserId, u.ItemId })
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(x => x.MigrationsHistoryTable("__MigrationsHistory"));
        }
    }
}
