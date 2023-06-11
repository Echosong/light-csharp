using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _0221 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "UniversitySubjects");

            migrationBuilder.DropColumn(
                name: "Constitution",
                table: "UniversityScores");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Universities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Builder",
                table: "Universities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Universities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Judgement",
                table: "Universities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendMajor",
                table: "Universities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Subject",
                table: "ScoreRankings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UniversityConstitutions",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    UpdaterId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UniversityConstitutions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "UniversityConstitutions");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Builder",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Judgement",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "RecommendMajor",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "ScoreRankings");

            migrationBuilder.AddColumn<string>(
                name: "Constitution",
                table: "UniversityScores",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UniversitySubjects",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdaterId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UniversitySubjects", x => x.Id);
                });
        }
    }
}
