using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class vehicleentered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_SchoolInfos_SchoolInfoId",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters");

            migrationBuilder.DropIndex(
                name: "IX_Waters_SchoolInfoId",
                table: "Waters");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_SchoolInfoId",
                table: "Electrics");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f85636b8-6d0a-44fe-bf6f-c7f62226aef0"), new Guid("7d3bebae-2db1-4ae9-a0a6-0375ee48f58d") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10609cf4-e11a-4a41-bce3-f4ccb68a925f"), new Guid("7d4cff82-eb87-4e8b-9b09-37884cedbf69") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0a9d2a35-b27e-4c27-8979-6e274782d51d"), new Guid("f1960ffa-1224-411c-a46c-7c3b6ef5f767") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a9d2a35-b27e-4c27-8979-6e274782d51d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10609cf4-e11a-4a41-bce3-f4ccb68a925f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f85636b8-6d0a-44fe-bf6f-c7f62226aef0"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7d3bebae-2db1-4ae9-a0a6-0375ee48f58d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7d4cff82-eb87-4e8b-9b09-37884cedbf69"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f1960ffa-1224-411c-a46c-7c3b6ef5f767"));

            migrationBuilder.DropColumn(
                name: "SchoolInfoId",
                table: "Waters");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "SchoolInfos");

            migrationBuilder.DropColumn(
                name: "SchoolInfoId",
                table: "Electrics");

            migrationBuilder.AddColumn<Guid>(
                name: "VehiclesId",
                table: "SchoolInfos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CampusVehicleEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CarsManagedByUniversity = table.Column<int>(type: "integer", nullable: false),
                    CarsEnteringUniversity = table.Column<int>(type: "integer", nullable: false),
                    MotorcyclesEnteringUniversity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusVehicleEntry", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolInfos_CampusVehicleEntry_VehiclesId",
                table: "SchoolInfos");

            migrationBuilder.DropTable(
                name: "CampusVehicleEntry");

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
                name: "SchoolInfoId",
                table: "Waters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "SchoolInfos",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolInfoId",
                table: "Electrics",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a9d2a35-b27e-4c27-8979-6e274782d51d"), null, "ADMIN", "ADMIN" },
                    { new Guid("10609cf4-e11a-4a41-bce3-f4ccb68a925f"), null, "SUPERADMIN", "SUPERADMIN" },
                    { new Guid("f85636b8-6d0a-44fe-bf6f-c7f62226aef0"), null, "USER", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("7d3bebae-2db1-4ae9-a0a6-0375ee48f58d"), 0, "571d287a-bd8d-4cc0-8a1e-c72edd1de904", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEPjtTjzmdlRLPvKDeuOfVZCwtKPqbYOIiRHzG9hkRUZhG83MsxrXiqR1CcbPuuKDJA==", null, false, null, "USER", false, "USER" },
                    { new Guid("7d4cff82-eb87-4e8b-9b09-37884cedbf69"), 0, "836d10a0-b1df-4e7a-bef8-cd448ec0880c", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEJ0FyCWf9HWeDUhu6VKgym40Xdy6Hi0QpTbizy3e1ladW7//6RhBkIK1aj736v2exg==", null, false, null, "USER", false, "SUPERADMIN" },
                    { new Guid("f1960ffa-1224-411c-a46c-7c3b6ef5f767"), 0, "4211cc1c-cba8-4ae4-bc0e-dfb27d2c3242", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEGTRGdgDNz1DGly+IQ0g4BO9sDk47fTW8bV9RE4CVEzrelkbWnXxB4TV4LaDCc3XcA==", null, false, null, "USER", false, "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("f85636b8-6d0a-44fe-bf6f-c7f62226aef0"), new Guid("7d3bebae-2db1-4ae9-a0a6-0375ee48f58d") },
                    { new Guid("10609cf4-e11a-4a41-bce3-f4ccb68a925f"), new Guid("7d4cff82-eb87-4e8b-9b09-37884cedbf69") },
                    { new Guid("0a9d2a35-b27e-4c27-8979-6e274782d51d"), new Guid("f1960ffa-1224-411c-a46c-7c3b6ef5f767") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Waters_SchoolInfoId",
                table: "Waters",
                column: "SchoolInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_SchoolInfoId",
                table: "Electrics",
                column: "SchoolInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_SchoolInfos_SchoolInfoId",
                table: "Electrics",
                column: "SchoolInfoId",
                principalTable: "SchoolInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters",
                column: "SchoolInfoId",
                principalTable: "SchoolInfos",
                principalColumn: "Id");
        }
    }
}
