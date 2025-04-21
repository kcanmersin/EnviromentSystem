using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dbec997d-da77-449d-9b08-82bdf8124e5d"), new Guid("0636ef7c-33e4-4dab-ae57-29e4e2eb79a7") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("52892b1c-bd00-4973-8774-12e8c8453fc5"), new Guid("3ce5f3f4-d48c-4d62-9cb5-11b67796ad62") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b49c2dc7-1285-4992-b4d7-7da184d1502e"), new Guid("93fd8c53-e5b6-499c-bc6a-49585052b522") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("52892b1c-bd00-4973-8774-12e8c8453fc5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b49c2dc7-1285-4992-b4d7-7da184d1502e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dbec997d-da77-449d-9b08-82bdf8124e5d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0636ef7c-33e4-4dab-ae57-29e4e2eb79a7"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3ce5f3f4-d48c-4d62-9cb5-11b67796ad62"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("93fd8c53-e5b6-499c-bc6a-49585052b522"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("06c8229f-1456-43e8-af7c-07b72021ae34"), null, "SUPERADMIN", "SUPERADMIN" },
                    { new Guid("4fd518d2-a4f7-4336-9ca9-71fdd7e47d6c"), null, "ADMIN", "ADMIN" },
                    { new Guid("942fd303-12d1-4ee8-995a-4048f91e8d3d"), null, "USER", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("403f6bad-5128-4330-9432-9a3a8d6f3842"), 0, "f7920823-3fa5-4d4c-8c6d-bd5318102d61", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAENpKN75UJxJrPUN5Kx8wDcGN/X/U1D8IvxboxdMtpXLvco+AzoWbXi0MlBYSM6jofg==", null, false, null, "USER", false, "USER" },
                    { new Guid("82a3fcbc-00c9-4d7c-a74c-9f288e46198f"), 0, "0f97c627-c393-4e49-bfdd-c192f8340c5a", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEOuAOwmKB1149mVJTQuXO6b4TS/svAkPVjlkTIiTmoR63BUnTHwo3u0xEQ21QTlWdQ==", null, false, null, "USER", false, "ADMIN" },
                    { new Guid("d1e75d8d-5518-4856-9fde-116c05da9d4b"), 0, "cbe2084c-53e8-4cc4-a91a-0a6769657751", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEAYagEtoE1Vjg77DJ1ocUFC3WXOQ3PzqXuSBw2fMjxfaKVKF8xMGh5Jc16jhoO3mvA==", null, false, null, "USER", false, "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("942fd303-12d1-4ee8-995a-4048f91e8d3d"), new Guid("403f6bad-5128-4330-9432-9a3a8d6f3842") },
                    { new Guid("4fd518d2-a4f7-4336-9ca9-71fdd7e47d6c"), new Guid("82a3fcbc-00c9-4d7c-a74c-9f288e46198f") },
                    { new Guid("06c8229f-1456-43e8-af7c-07b72021ae34"), new Guid("d1e75d8d-5518-4856-9fde-116c05da9d4b") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("942fd303-12d1-4ee8-995a-4048f91e8d3d"), new Guid("403f6bad-5128-4330-9432-9a3a8d6f3842") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4fd518d2-a4f7-4336-9ca9-71fdd7e47d6c"), new Guid("82a3fcbc-00c9-4d7c-a74c-9f288e46198f") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("06c8229f-1456-43e8-af7c-07b72021ae34"), new Guid("d1e75d8d-5518-4856-9fde-116c05da9d4b") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("06c8229f-1456-43e8-af7c-07b72021ae34"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4fd518d2-a4f7-4336-9ca9-71fdd7e47d6c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("942fd303-12d1-4ee8-995a-4048f91e8d3d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("403f6bad-5128-4330-9432-9a3a8d6f3842"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("82a3fcbc-00c9-4d7c-a74c-9f288e46198f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d1e75d8d-5518-4856-9fde-116c05da9d4b"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("52892b1c-bd00-4973-8774-12e8c8453fc5"), null, "USER", "USER" },
                    { new Guid("b49c2dc7-1285-4992-b4d7-7da184d1502e"), null, "SUPERADMIN", "SUPERADMIN" },
                    { new Guid("dbec997d-da77-449d-9b08-82bdf8124e5d"), null, "ADMIN", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("0636ef7c-33e4-4dab-ae57-29e4e2eb79a7"), 0, "66e1498b-f716-4582-9802-e8bf568d91f1", "admin@example.com", false, true, false, null, "ADMIN", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAELG2DDoGCNEKQMbsMDFWQNhvPYlg2V9HoIPtHp8ptZsx+trIta7AYe1AkQAXq8/Waw==", null, false, null, "USER", false, "ADMIN" },
                    { new Guid("3ce5f3f4-d48c-4d62-9cb5-11b67796ad62"), 0, "2429cb42-76e1-4aa4-9131-a20229eae32c", "user@example.com", false, true, false, null, "USER", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEGmexe5gClssvalRm3ZdIIzCZVRaXZULBzRbOe2GzVGxOyhIwm6OLpTQVk77uO/rYw==", null, false, null, "USER", false, "USER" },
                    { new Guid("93fd8c53-e5b6-499c-bc6a-49585052b522"), 0, "acbe9ec7-3e0d-46e7-8129-358821b2aefd", "superadmin@example.com", false, true, false, null, "SUPERADMIN", "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEIsRGQzcRg8XiFDCQbbM63fmpjxa8cLb/qNlXVoHa0Adq0Gk7YGQJuo7eG4UFpxt/Q==", null, false, null, "USER", false, "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("dbec997d-da77-449d-9b08-82bdf8124e5d"), new Guid("0636ef7c-33e4-4dab-ae57-29e4e2eb79a7") },
                    { new Guid("52892b1c-bd00-4973-8774-12e8c8453fc5"), new Guid("3ce5f3f4-d48c-4d62-9cb5-11b67796ad62") },
                    { new Guid("b49c2dc7-1285-4992-b4d7-7da184d1502e"), new Guid("93fd8c53-e5b6-499c-bc6a-49585052b522") }
                });
        }
    }
}
