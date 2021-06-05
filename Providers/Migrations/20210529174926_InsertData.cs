// <copyright file="20210529174926_InsertData.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Providers.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// Documentation of InsertData.
    /// </summary>
    public partial class InsertData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var user1Id = Guid.NewGuid().ToString();

            migrationBuilder.InsertData(
                table: "Users",
                columns: new string[] { "Id", "LastName", "FirstName", "EmailAddress", "Password" },
                values: new string[] { user1Id, "Test", "Test", "test@myaccess.com", "1000:B4FQYp0tpIskbdjllRweb6G4kCwXNJ/Y:3BBKBbaJSQ/y1DD7wEw1H0UByWnl5Dhs" });

            var systemMyAccessId = Guid.NewGuid().ToString();

            migrationBuilder.InsertData(
                table: "Systems",
                columns: new string[] { "Id", "Name" },
                values: new string[] { systemMyAccessId, "MyAccess" });

            var typesMenusId = Guid.NewGuid().ToString();
            var permissionsMenusId = Guid.NewGuid().ToString();
            var profilesMenusId = Guid.NewGuid().ToString();

            var valuesTypes = new object[,]
            {
                { typesMenusId, "Menus" },
                { permissionsMenusId, "Permissions" },
                { profilesMenusId, "Profiles" },
            };

            migrationBuilder.InsertData(
                table: "Types",
                columns: new string[] { "Id", "Name" },
                values: valuesTypes);

            var usersItemsId = Guid.NewGuid().ToString();
            var systemsItemsId = Guid.NewGuid().ToString();
            var typesItemsId = Guid.NewGuid().ToString();
            var itemsItemsId = Guid.NewGuid().ToString();

            var valuesItems = new object[,]
            {
                { usersItemsId, systemMyAccessId, typesMenusId, "Users" },
                { systemsItemsId, systemMyAccessId, typesMenusId, "Systems" },
                { typesItemsId, systemMyAccessId, typesMenusId, "Types" },
                { itemsItemsId, systemMyAccessId, typesMenusId, "Items" },
            };

            migrationBuilder.InsertData(
                table: "Items",
                columns: new string[] { "Id", "SystemId", "TypeId", "Name" },
                values: valuesItems);

            var valuesUsersItems = new object[,]
            {
                { Guid.NewGuid().ToString(), user1Id, usersItemsId },
                { Guid.NewGuid().ToString(), user1Id, systemsItemsId },
                { Guid.NewGuid().ToString(), user1Id, typesItemsId },
                { Guid.NewGuid().ToString(), user1Id, itemsItemsId },
            };

            migrationBuilder.InsertData(
                table: "UsersItems",
                columns: new string[] { "Id", "UserId", "ItemId" },
                values: valuesUsersItems);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
