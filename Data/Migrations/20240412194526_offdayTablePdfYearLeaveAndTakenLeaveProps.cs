using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class offdayTablePdfYearLeaveAndTakenLeaveProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("1e02f1f2-a4a4-4eab-be8a-67d5cc51be12"));

            migrationBuilder.AddColumn<double>(
                name: "PdfRemainTakenLeave",
                table: "OffDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PdfRemainYearLeave",
                table: "OffDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PdfUsedYearLeave",
                table: "OffDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("c5a3dc3d-ce8b-4e7c-af1e-01d9866048ef"), new DateTime(2024, 4, 12, 22, 45, 26, 599, DateTimeKind.Local).AddTicks(6008), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 12, 22, 45, 26, 599, DateTimeKind.Local).AddTicks(6018), "superadmin", 3, 0, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("c5a3dc3d-ce8b-4e7c-af1e-01d9866048ef"));

            migrationBuilder.DropColumn(
                name: "PdfRemainTakenLeave",
                table: "OffDays");

            migrationBuilder.DropColumn(
                name: "PdfRemainYearLeave",
                table: "OffDays");

            migrationBuilder.DropColumn(
                name: "PdfUsedYearLeave",
                table: "OffDays");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("1e02f1f2-a4a4-4eab-be8a-67d5cc51be12"), new DateTime(2024, 4, 8, 2, 16, 19, 36, DateTimeKind.Local).AddTicks(2571), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 8, 2, 16, 19, 36, DateTimeKind.Local).AddTicks(2582), "superadmin", 3, 0, "superadmin" });
        }
    }
}
