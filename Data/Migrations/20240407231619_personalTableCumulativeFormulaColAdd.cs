using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class personalTableCumulativeFormulaColAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("2d09c162-d749-46ce-8cf6-b681c35b02d1"));

            migrationBuilder.AddColumn<string>(
                name: "CumulativeFormula",
                table: "Personals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("1e02f1f2-a4a4-4eab-be8a-67d5cc51be12"), new DateTime(2024, 4, 8, 2, 16, 19, 36, DateTimeKind.Local).AddTicks(2571), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 8, 2, 16, 19, 36, DateTimeKind.Local).AddTicks(2582), "superadmin", 3, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("1e02f1f2-a4a4-4eab-be8a-67d5cc51be12"));

            migrationBuilder.DropColumn(
                name: "CumulativeFormula",
                table: "Personals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("2d09c162-d749-46ce-8cf6-b681c35b02d1"), new DateTime(2024, 4, 6, 0, 40, 7, 695, DateTimeKind.Local).AddTicks(4273), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 6, 0, 40, 7, 695, DateTimeKind.Local).AddTicks(4286), "superadmin", 3, 0, "superadmin" });
        }
    }
}
