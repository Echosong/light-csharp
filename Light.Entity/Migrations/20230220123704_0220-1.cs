using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _02201 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "ScoreRankings",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoreRankings",
                table: "ScoreRankings");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "ScoreRankings");


        }
    }
}
