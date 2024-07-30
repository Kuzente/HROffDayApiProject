using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class userlogfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("67461e8c-95ac-47f7-99f3-9db32e29aa82"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("f5e22260-81ab-4d8f-9366-b7b724d4665a"), new DateTime(2024, 7, 29, 20, 19, 8, 14, DateTimeKind.Local).AddTicks(7984), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 7, 29, 20, 19, 8, 14, DateTimeKind.Local).AddTicks(7992), "superadmin", 3, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("f5e22260-81ab-4d8f-9366-b7b724d4665a"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("67461e8c-95ac-47f7-99f3-9db32e29aa82"), new DateTime(2024, 7, 29, 20, 8, 19, 3, DateTimeKind.Local).AddTicks(9317), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 7, 29, 20, 8, 19, 3, DateTimeKind.Local).AddTicks(9327), "superadmin", 3, 0, "superadmin" });
        }
    }
}
