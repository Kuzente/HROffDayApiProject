using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class personalCumulativeTableAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("c5a3dc3d-ce8b-4e7c-af1e-01d9866048ef"));

            migrationBuilder.CreateTable(
                name: "PersonalCumulatives",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    EarnedYearLeave = table.Column<int>(type: "int", nullable: false),
                    RemainYearLeave = table.Column<int>(type: "int", nullable: false),
                    IsReportCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsNotificationExist = table.Column<bool>(type: "bit", nullable: false),
                    Personal_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCumulatives", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonalCumulatives_Personals_Personal_Id",
                        column: x => x.Personal_Id,
                        principalTable: "Personals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("d3f934a8-0661-41ae-90ab-05aa5c144cb3"), new DateTime(2024, 4, 25, 18, 9, 16, 995, DateTimeKind.Local).AddTicks(3095), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 25, 18, 9, 16, 995, DateTimeKind.Local).AddTicks(3104), "superadmin", 3, 0, "superadmin" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCumulatives_Personal_Id",
                table: "PersonalCumulatives",
                column: "Personal_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalCumulatives");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: new Guid("d3f934a8-0661-41ae-90ab-05aa5c144cb3"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Email", "IsDefaultPassword", "ModifiedAt", "Password", "Role", "Status", "Username" },
                values: new object[] { new Guid("c5a3dc3d-ce8b-4e7c-af1e-01d9866048ef"), new DateTime(2024, 4, 12, 22, 45, 26, 599, DateTimeKind.Local).AddTicks(6008), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@superadmin.com", true, new DateTime(2024, 4, 12, 22, 45, 26, 599, DateTimeKind.Local).AddTicks(6018), "superadmin", 3, 0, "superadmin" });
        }
    }
}
