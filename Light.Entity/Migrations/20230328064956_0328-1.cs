using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations
{
    public partial class _03281 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanMajorIds",
                table: "UserVoluntaryDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");



            migrationBuilder.AddColumn<int>(
                name: "PlanMajorId",
                table: "UniversityScoreTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanMajorIds",
                table: "UserVoluntaryDetails");


            migrationBuilder.DropColumn(
                name: "PlanMajorId",
                table: "UniversityScoreTags");

        }
    }
}
