using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class personelTableYearLeaveDateAndIsYearLeaveRetiredPropertyAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("66397921-2ed6-4b98-b220-fc4ac1dfa7fa"));

            migrationBuilder.AddColumn<bool>(
                name: "IsYearLeaveRetired",
                table: "Personals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "YearLeaveDate",
                table: "Personals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("d3a9bf02-3988-46d0-8bc8-9d0c1f0d9234"), new DateTime(2024, 4, 3, 18, 0, 55, 103, DateTimeKind.Local).AddTicks(4445), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 3, 18, 0, 55, 103, DateTimeKind.Local).AddTicks(4460), "superadmin", 0, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("d3a9bf02-3988-46d0-8bc8-9d0c1f0d9234"));

            migrationBuilder.DropColumn(
                name: "IsYearLeaveRetired",
                table: "Personals");

            migrationBuilder.DropColumn(
                name: "YearLeaveDate",
                table: "Personals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("66397921-2ed6-4b98-b220-fc4ac1dfa7fa"), new DateTime(2024, 4, 3, 14, 54, 21, 698, DateTimeKind.Local).AddTicks(5961), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 3, 14, 54, 21, 698, DateTimeKind.Local).AddTicks(5970), "superadmin", 0, 0, "superadmin" });
        }
    }
}
