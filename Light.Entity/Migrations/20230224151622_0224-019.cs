using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _0224019 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int>(
                name: "Batch",
                table: "UserVoluntaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "UserVoluntaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniversityCount",
                table: "UserVoluntaries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Batch",
                table: "UserVoluntaries");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserVoluntaries");

            migrationBuilder.DropColumn(
                name: "UniversityCount",
                table: "UserVoluntaries");
        }
    }
}
