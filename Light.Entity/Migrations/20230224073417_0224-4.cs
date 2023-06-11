using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _02244 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "UnvisersityScoreId",
                table: "UserVoluntaryDetails",
                newName: "PlanSchoolId");

            migrationBuilder.RenameColumn(
                name: "UnvisersityMajorScoreId",
                table: "UserVoluntaryDetails",
                newName: "UniversityMajorScoreId");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "UniversityAttentions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "UniversityAttentions");

            migrationBuilder.RenameColumn(
                name: "PlanSchoolId",
                table: "UserVoluntaryDetails",
                newName: "UnvisersityScoreId");

            migrationBuilder.RenameColumn(
                name: "UniversityMajorScoreId",
                table: "UserVoluntaryDetails",
                newName: "UnvisersityMajorScoreId");
        }
    }
}
