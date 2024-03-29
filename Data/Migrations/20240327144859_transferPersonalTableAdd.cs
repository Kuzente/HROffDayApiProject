using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class transferPersonalTableAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferPersonals",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personal_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferPersonals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransferPersonals_Personals_Personal_Id",
                        column: x => x.Personal_Id,
                        principalTable: "Personals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferPersonals_Personal_Id",
                table: "TransferPersonals",
                column: "Personal_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferPersonals");
        }
    }
}
