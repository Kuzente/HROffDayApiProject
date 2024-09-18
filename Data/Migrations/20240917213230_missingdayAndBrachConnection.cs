using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class missingdayAndBrachConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("964311b4-df08-4216-be77-e6e4a87fe3fd"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "MailVerificationToken", "ModifiedAt", "Password", "Role", "Status", "TokenExpiredDate", "Username" },
                values: new object[] { new Guid("e6bb3a30-0785-4a5c-a50a-395ab609907d"), new DateTime(2024, 9, 18, 0, 32, 30, 154, DateTimeKind.Local).AddTicks(631), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, "-", new DateTime(2024, 9, 18, 0, 32, 30, 154, DateTimeKind.Local).AddTicks(642), "superadmin", 3, 0, new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "superadmin" });

            migrationBuilder.CreateIndex(
                name: "IX_MissingDays_Branch_Id",
                table: "MissingDays",
                column: "Branch_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MissingDays_Branches_Branch_Id",
                table: "MissingDays",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissingDays_Branches_Branch_Id",
                table: "MissingDays");

            migrationBuilder.DropIndex(
                name: "IX_MissingDays_Branch_Id",
                table: "MissingDays");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("e6bb3a30-0785-4a5c-a50a-395ab609907d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "MailVerificationToken", "ModifiedAt", "Password", "Role", "Status", "TokenExpiredDate", "Username" },
                values: new object[] { new Guid("964311b4-df08-4216-be77-e6e4a87fe3fd"), new DateTime(2024, 8, 3, 4, 13, 6, 829, DateTimeKind.Local).AddTicks(2610), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, "-", new DateTime(2024, 8, 3, 4, 13, 6, 829, DateTimeKind.Local).AddTicks(2621), "superadmin", 3, 0, new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "superadmin" });
        }
    }
}
