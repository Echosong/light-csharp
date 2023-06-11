using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Light.Entity.Migrations
{
    public partial class _0329 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "UserVoluntaryDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "UserVoluntaryDetails");
        }
    }
}
