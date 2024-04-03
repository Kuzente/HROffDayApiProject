using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class offdaysHrAndDirectorNameFieldAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("15db391f-8823-4b1a-8139-b5a979abb3fc"));

            migrationBuilder.AddColumn<string>(
                name: "DirectorName",
                table: "OffDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrName",
                table: "OffDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("66397921-2ed6-4b98-b220-fc4ac1dfa7fa"), new DateTime(2024, 4, 3, 14, 54, 21, 698, DateTimeKind.Local).AddTicks(5961), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 3, 14, 54, 21, 698, DateTimeKind.Local).AddTicks(5970), "superadmin", 0, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("66397921-2ed6-4b98-b220-fc4ac1dfa7fa"));

            migrationBuilder.DropColumn(
                name: "DirectorName",
                table: "OffDays");

            migrationBuilder.DropColumn(
                name: "HrName",
                table: "OffDays");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("15db391f-8823-4b1a-8139-b5a979abb3fc"), new DateTime(2024, 3, 31, 16, 58, 16, 875, DateTimeKind.Local).AddTicks(5991), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 3, 31, 16, 58, 16, 875, DateTimeKind.Local).AddTicks(6002), "superadmin", 0, 0, "superadmin" });
        }
    }
}
