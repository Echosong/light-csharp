using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations
{
    public partial class _03192 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanSchoolId",
                table: "UserVoluntaryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanMajorID",
                table: "UniversityMajorScores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanSchoolId",
                table: "UserVoluntaryDetails");

            migrationBuilder.DropColumn(
                name: "PlanMajorID",
                table: "UniversityMajorScores");
        }
    }
}
