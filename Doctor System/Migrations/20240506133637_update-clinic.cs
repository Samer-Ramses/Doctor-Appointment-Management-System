using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor_System.Migrations
{
    /// <inheritdoc />
    public partial class updateclinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHoursId",
                table: "Clinics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingHoursId",
                table: "Clinics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
