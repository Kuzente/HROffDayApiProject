using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class totalTakenLeaveIINTToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("d3a9bf02-3988-46d0-8bc8-9d0c1f0d9234"));

            migrationBuilder.AlterColumn<double>(
                name: "TotalTakenLeave",
                table: "Personals",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("2d09c162-d749-46ce-8cf6-b681c35b02d1"), new DateTime(2024, 4, 6, 0, 40, 7, 695, DateTimeKind.Local).AddTicks(4273), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 6, 0, 40, 7, 695, DateTimeKind.Local).AddTicks(4286), "superadmin", 3, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("2d09c162-d749-46ce-8cf6-b681c35b02d1"));

            migrationBuilder.AlterColumn<int>(
                name: "TotalTakenLeave",
                table: "Personals",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("d3a9bf02-3988-46d0-8bc8-9d0c1f0d9234"), new DateTime(2024, 4, 3, 18, 0, 55, 103, DateTimeKind.Local).AddTicks(4445), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 3, 18, 0, 55, 103, DateTimeKind.Local).AddTicks(4460), "superadmin", 0, 0, "superadmin" });
        }
    }
}
