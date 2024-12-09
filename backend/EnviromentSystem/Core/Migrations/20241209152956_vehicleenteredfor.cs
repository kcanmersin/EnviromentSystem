using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class vehicleenteredfor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolInfos_CampusVehicleEntry_VehiclesId",
                table: "SchoolInfos");

            migrationBuilder.DropIndex(
                name: "IX_SchoolInfos_VehiclesId",
                table: "SchoolInfos");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("12999e3e-bc71-43a5-8c80-52a2a18f21b3"), new Guid("16503931-3912-4d30-868a-fb0beefe59d1") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e1dfa41b-dee4-4913-a4e1-776bafea8415"), new Guid("21532e24-dd4b-4fbf-8a38-bf8677037caf") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e7489fd1-3651-4064-ae94-a3bba37b97ad"), new Guid("681eca24-03f6-4aed-b304-581944ce1099") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12999e3e-bc71-43a5-8c80-52a2a18f21b3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e1dfa41b-dee4-4913-a4e1-776bafea8415"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e7489fd1-3651-4064-ae94-a3bba37b97ad"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("16503931-3912-4d30-868a-fb0beefe59d1"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("21532e24-dd4b-4fbf-8a38-bf8677037caf"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("681eca24-03f6-4aed-b304-581944ce1099"));

            migrationBuilder.DropColumn(
                name: "VehiclesId",
                table: "SchoolInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "CampusVehicleEntryId",
                table: "SchoolInfos",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("988963dd-fe67-4ff4-b962-0e9472f23816"), null, "USER", "USER" },
                    { new Guid("ac2c246c-b3db-42d3-8daf-e41e7aa2a9ae"), null, "SUPERADMIN", "SUPERADMIN" },
                    { new Guid("baf90810-2a87-4478-8e80-9c28161172d3"), null, "ADMIN", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("646dc2b1-7527-414c-919c-e9c6bea2c78d"), 0, "52cad739-530a-4ecd-883f-54c5826b8479", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAENTBVK5LnomMb5gmgNmpGDRotdcSW/ONIJYUTlTA2KmhJujkXS7716nR5S8iY2ceEw==", null, false, null, "USER", false, "USER" },
                    { new Guid("6c7e22e1-4bc2-419c-9c86-73774435f98f"), 0, "688620d7-3b7b-4630-b562-5c28c493ae12", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEBygMmmtBifRrTCaNtlbzSxJOCXpp3sQfZiT094uBNLK+aN50lzW1D2+DD2qOhOnmA==", null, false, null, "USER", false, "SUPERADMIN" },
                    { new Guid("716d5945-db78-408e-8ef0-6c0cebbadc83"), 0, "50789197-f509-46a9-a181-8c077236c07f", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEPkwZEhIEW0C1p0dqa6mZj2XtCy7dT7rHAdKJdvv0aCy1duRrk/YrIXo8eJ4gnw5ZQ==", null, false, null, "USER", false, "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("988963dd-fe67-4ff4-b962-0e9472f23816"), new Guid("646dc2b1-7527-414c-919c-e9c6bea2c78d") },
                    { new Guid("ac2c246c-b3db-42d3-8daf-e41e7aa2a9ae"), new Guid("6c7e22e1-4bc2-419c-9c86-73774435f98f") },
                    { new Guid("baf90810-2a87-4478-8e80-9c28161172d3"), new Guid("716d5945-db78-408e-8ef0-6c0cebbadc83") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolInfos_CampusVehicleEntryId",
                table: "SchoolInfos",
                column: "CampusVehicleEntryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolInfos_CampusVehicleEntry_CampusVehicleEntryId",
                table: "SchoolInfos",
                column: "CampusVehicleEntryId",
                principalTable: "CampusVehicleEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolInfos_CampusVehicleEntry_CampusVehicleEntryId",
                table: "SchoolInfos");

            migrationBuilder.DropIndex(
                name: "IX_SchoolInfos_CampusVehicleEntryId",
                table: "SchoolInfos");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("988963dd-fe67-4ff4-b962-0e9472f23816"), new Guid("646dc2b1-7527-414c-919c-e9c6bea2c78d") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ac2c246c-b3db-42d3-8daf-e41e7aa2a9ae"), new Guid("6c7e22e1-4bc2-419c-9c86-73774435f98f") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("baf90810-2a87-4478-8e80-9c28161172d3"), new Guid("716d5945-db78-408e-8ef0-6c0cebbadc83") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("988963dd-fe67-4ff4-b962-0e9472f23816"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ac2c246c-b3db-42d3-8daf-e41e7aa2a9ae"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("baf90810-2a87-4478-8e80-9c28161172d3"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("646dc2b1-7527-414c-919c-e9c6bea2c78d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6c7e22e1-4bc2-419c-9c86-73774435f98f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("716d5945-db78-408e-8ef0-6c0cebbadc83"));

            migrationBuilder.DropColumn(
                name: "CampusVehicleEntryId",
                table: "SchoolInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "VehiclesId",
                table: "SchoolInfos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("12999e3e-bc71-43a5-8c80-52a2a18f21b3"), null, "ADMIN", "ADMIN" },
                    { new Guid("e1dfa41b-dee4-4913-a4e1-776bafea8415"), null, "USER", "USER" },
                    { new Guid("e7489fd1-3651-4064-ae94-a3bba37b97ad"), null, "SUPERADMIN", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("16503931-3912-4d30-868a-fb0beefe59d1"), 0, "31b3450e-9934-4f0e-9ce5-d0416f3dfc9c", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEC+TCOqH++x5ryL+W+G3KQXaU6w55GP5TN3cGOzimrfuG8viSZKHYmmmdQ7Ii0NuDQ==", null, false, null, "USER", false, "ADMIN" },
                    { new Guid("21532e24-dd4b-4fbf-8a38-bf8677037caf"), 0, "a8ebbdf7-741e-4e9e-879e-ff4206f65c14", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEMITI7cxvYZfHDERit2bdUA+Iv424OYP95HP1w2LuGUH9rgz3NHXWlH9HLMy9KXKiw==", null, false, null, "USER", false, "USER" },
                    { new Guid("681eca24-03f6-4aed-b304-581944ce1099"), 0, "69fcc667-757e-49f8-b98e-092395bc792e", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEL9+Qy4afcnXJm2cb/dnn8fHPbK2ndX31dZquWftRVKnQ9R9z1Sj/kTnYeGYE6C4jQ==", null, false, null, "USER", false, "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("12999e3e-bc71-43a5-8c80-52a2a18f21b3"), new Guid("16503931-3912-4d30-868a-fb0beefe59d1") },
                    { new Guid("e1dfa41b-dee4-4913-a4e1-776bafea8415"), new Guid("21532e24-dd4b-4fbf-8a38-bf8677037caf") },
                    { new Guid("e7489fd1-3651-4064-ae94-a3bba37b97ad"), new Guid("681eca24-03f6-4aed-b304-581944ce1099") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolInfos_VehiclesId",
                table: "SchoolInfos",
                column: "VehiclesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolInfos_CampusVehicleEntry_VehiclesId",
                table: "SchoolInfos",
                column: "VehiclesId",
                principalTable: "CampusVehicleEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
