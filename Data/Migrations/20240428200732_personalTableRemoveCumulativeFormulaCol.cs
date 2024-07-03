using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class personalTableRemoveCumulativeFormulaCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("d3f934a8-0661-41ae-90ab-05aa5c144cb3"));

            migrationBuilder.DropColumn(
                name: "CumulativeFormula",
                table: "Personals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("0e74704a-bfca-4236-aa38-14a317567123"), new DateTime(2024, 4, 28, 23, 7, 32, 151, DateTimeKind.Local).AddTicks(6847), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 28, 23, 7, 32, 151, DateTimeKind.Local).AddTicks(6856), "superadmin", 3, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("0e74704a-bfca-4236-aa38-14a317567123"));

            migrationBuilder.AddColumn<string>(
                name: "CumulativeFormula",
                table: "Personals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("d3f934a8-0661-41ae-90ab-05aa5c144cb3"), new DateTime(2024, 4, 25, 18, 9, 16, 995, DateTimeKind.Local).AddTicks(3095), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 25, 18, 9, 16, 995, DateTimeKind.Local).AddTicks(3104), "superadmin", 3, 0, "superadmin" });
        }
    }
}
