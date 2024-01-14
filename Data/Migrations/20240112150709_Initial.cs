using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Personals",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalYearLeave = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartJobDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndJobDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistirationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetiredOrOld = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedYearLeave = table.Column<int>(type: "int", nullable: false),
                    Branch_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetails_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Personals_Branches_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personals_Positions_Position_Id",
                        column: x => x.Position_Id,
                        principalTable: "Positions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffDays",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountLeave = table.Column<int>(type: "int", nullable: false),
                    LeaveByWeek = table.Column<int>(type: "int", nullable: false),
                    LeaveByYear = table.Column<int>(type: "int", nullable: false),
                    LeaveByPublicHoliday = table.Column<int>(type: "int", nullable: false),
                    LeaveByFreeDay = table.Column<int>(type: "int", nullable: false),
                    LeaveByTaken = table.Column<int>(type: "int", nullable: false),
                    LeaveByTravel = table.Column<int>(type: "int", nullable: false),
                    LeaveByDead = table.Column<int>(type: "int", nullable: false),
                    LeaveByFather = table.Column<int>(type: "int", nullable: false),
                    LeaveByMarried = table.Column<int>(type: "int", nullable: false),
                    OffDayStatus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personal_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffDays", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OffDays_Personals_Personal_Id",
                        column: x => x.Personal_Id,
                        principalTable: "Personals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Handicapped = table.Column<bool>(type: "bit", nullable: false),
                    EducationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GraduatedSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<double>(type: "float", nullable: true),
                    SgkCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SskNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodySize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personal_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_Personals_Personal_Id",
                        column: x => x.Personal_Id,
                        principalTable: "Personals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OffDays_Personal_Id",
                table: "OffDays",
                column: "Personal_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_Personal_Id",
                table: "PersonalDetails",
                column: "Personal_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personals_Branch_Id",
                table: "Personals",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Personals_Position_Id",
                table: "Personals",
                column: "Position_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OffDays");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropTable(
                name: "Personals");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
