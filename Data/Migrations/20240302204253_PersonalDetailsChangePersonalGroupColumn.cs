using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class PersonalDetailsChangePersonalGroupColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GraduatedSchool",
                table: "PersonalDetails",
                newName: "PersonalGroup");

            migrationBuilder.AlterColumn<double>(
                name: "Salary",
                table: "PersonalDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalGroup",
                table: "PersonalDetails",
                newName: "GraduatedSchool");

            migrationBuilder.AlterColumn<double>(
                name: "Salary",
                table: "PersonalDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
