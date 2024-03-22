using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class personalIsBackToWorkAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OffDays_Branches_Branch_Id",
                table: "OffDays");

            migrationBuilder.DropForeignKey(
                name: "FK_OffDays_Positions_Position_Id",
                table: "OffDays");

            migrationBuilder.DropIndex(
                name: "IX_OffDays_Branch_Id",
                table: "OffDays");

            migrationBuilder.DropIndex(
                name: "IX_OffDays_Position_Id",
                table: "OffDays");

            migrationBuilder.RenameColumn(
                name: "Position_Id",
                table: "OffDays",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "Branch_Id",
                table: "OffDays",
                newName: "BranchId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBackToWork",
                table: "Personals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBackToWork",
                table: "Personals");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "OffDays",
                newName: "Position_Id");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "OffDays",
                newName: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OffDays_Branch_Id",
                table: "OffDays",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OffDays_Position_Id",
                table: "OffDays",
                column: "Position_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OffDays_Branches_Branch_Id",
                table: "OffDays",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OffDays_Positions_Position_Id",
                table: "OffDays",
                column: "Position_Id",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
