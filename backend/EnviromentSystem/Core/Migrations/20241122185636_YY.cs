using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class YY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6"), new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181"), new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03"), new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede"));

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Waters",
                newName: "Usage");

            migrationBuilder.RenameColumn(
                name: "Consumption",
                table: "Waters",
                newName: "InitialMeterValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolInfoId",
                table: "Waters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Waters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "FinalMeterValue",
                table: "Waters",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters",
                column: "SchoolInfoId",
                principalTable: "SchoolInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters");

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
                name: "Date",
                table: "Waters");

            migrationBuilder.DropColumn(
                name: "FinalMeterValue",
                table: "Waters");

            migrationBuilder.RenameColumn(
                name: "Usage",
                table: "Waters",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "InitialMeterValue",
                table: "Waters",
                newName: "Consumption");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolInfoId",
                table: "Waters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6"), null, "USER", "USER" },
                    { new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03"), null, "ADMIN", "ADMIN" },
                    { new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181"), null, "SUPERADMIN", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837"), 0, "473eb3ce-981a-4658-afae-36127e3c306f", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAELDO86DJGKCQVJVT4EecFwA3Za8B3fO5YGfQRFsOl6aMLfOh93ThDFUujEMuy+gQWg==", null, false, null, "USER", false, "USER" },
                    { new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb"), 0, "4a910417-eb6c-4ef9-9ff9-dee07497ac07", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEIiukV3m/Zw0bAI6oBQekZjCe30a4ma/vxkaCImBuiByJGNbaMhgMjElqmUblcJ0+Q==", null, false, null, "USER", false, "SUPERADMIN" },
                    { new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede"), 0, "6c57829f-22fd-4702-a3f2-87a5f4b04ab7", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEFMXqcHmPsKRLpWFLCf8N/JtduPHR27eb/50TxAj3DKk2U2560vyE9+PuLKZysuPTQ==", null, false, null, "USER", false, "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("166a4262-32fa-4368-898d-d41e2cb08fb6"), new Guid("167326c1-11c3-494c-aa9b-9e0e42ee4837") },
                    { new Guid("eb9ecb38-c6e9-42d9-9295-ce2c307b0181"), new Guid("82140c4f-784b-4af9-bb73-f52f019f7beb") },
                    { new Guid("4418aba5-5187-4c26-b0bc-c41ced161e03"), new Guid("83c85d03-cb46-4ebb-b52f-315eaad91ede") }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Waters_SchoolInfos_SchoolInfoId",
                table: "Waters",
                column: "SchoolInfoId",
                principalTable: "SchoolInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
