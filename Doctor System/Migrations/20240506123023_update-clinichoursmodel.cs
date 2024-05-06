using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor_System.Migrations
{
    /// <inheritdoc />
    public partial class updateclinichoursmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClinicsWorkingHours",
                table: "ClinicsWorkingHours");

            migrationBuilder.DropIndex(
                name: "IX_ClinicsWorkingHours_ClinicId",
                table: "ClinicsWorkingHours");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClinicsWorkingHours");

            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "ClinicsWorkingHours",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClinicsWorkingHours",
                table: "ClinicsWorkingHours",
                columns: new[] { "ClinicId", "DayOfWeek" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClinicsWorkingHours",
                table: "ClinicsWorkingHours");

            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "ClinicsWorkingHours",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClinicsWorkingHours",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClinicsWorkingHours",
                table: "ClinicsWorkingHours",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicsWorkingHours_ClinicId",
                table: "ClinicsWorkingHours",
                column: "ClinicId");
        }
    }
}
