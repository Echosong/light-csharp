using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _02242 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniversitiesAttention",
                table: "UniversitiesAttention");

            migrationBuilder.RenameTable(
                name: "UniversitiesAttention",
                newName: "UniversityAttentions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniversityAttentions",
                table: "UniversityAttentions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniversityAttentions",
                table: "UniversityAttentions");

            migrationBuilder.RenameTable(
                name: "UniversityAttentions",
                newName: "UniversitiesAttention");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniversitiesAttention",
                table: "UniversitiesAttention",
                column: "Id");
        }
    }
}
