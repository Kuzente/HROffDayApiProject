using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "MailVerificationToken", "ModifiedAt", "Password", "Role", "Status", "TokenExpiredDate", "Username" },
                values: new object[] { new Guid("964311b4-df08-4216-be77-e6e4a87fe3fd"), new DateTime(2024, 8, 3, 4, 13, 6, 829, DateTimeKind.Local).AddTicks(2610), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, "-", new DateTime(2024, 8, 3, 4, 13, 6, 829, DateTimeKind.Local).AddTicks(2621), "superadmin", 3, 0, new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("964311b4-df08-4216-be77-e6e4a87fe3fd"));
        }
    }
}
