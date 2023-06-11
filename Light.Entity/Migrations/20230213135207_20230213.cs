using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations {
    public partial class _20230213 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Is211",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "Is985",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "IsDoubleFirst",
                table: "Universities");

            migrationBuilder.AddColumn<string>(
                name: "AreaName",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Majors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Majors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Experts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "AreaName",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Majors");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Majors");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Experts");

            migrationBuilder.AddColumn<bool>(
                name: "Is211",
                table: "Universities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is985",
                table: "Universities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoubleFirst",
                table: "Universities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
