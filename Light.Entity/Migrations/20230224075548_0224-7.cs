using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _02247 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UniversityPlans");

            migrationBuilder.AddColumn<float>(
                name: "Fee",
                table: "UniversityPlans",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "UniversityPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MajorName",
                table: "UniversityPlans",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlanCount",
                table: "UniversityPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "UniversityPlans");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "UniversityPlans");

            migrationBuilder.DropColumn(
                name: "MajorName",
                table: "UniversityPlans");

            migrationBuilder.DropColumn(
                name: "PlanCount",
                table: "UniversityPlans");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UniversityPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
