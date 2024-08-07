using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class userEntityTokenAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("f5e22260-81ab-4d8f-9366-b7b724d4665a"));

            migrationBuilder.AddColumn<string>(
                name: "MailVerificationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailVerificationToken",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("f5e22260-81ab-4d8f-9366-b7b724d4665a"), new DateTime(2024, 7, 29, 20, 19, 8, 14, DateTimeKind.Local).AddTicks(7984), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 7, 29, 20, 19, 8, 14, DateTimeKind.Local).AddTicks(7992), "superadmin", 3, 0, "superadmin" });
        }
    }
}
