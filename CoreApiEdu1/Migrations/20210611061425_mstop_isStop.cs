using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class mstop_isStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "minutes",
                table: "MStops");

            migrationBuilder.RenameColumn(
                name: "finish",
                table: "MStops",
                newName: "date");

            migrationBuilder.AddColumn<bool>(
                name: "isStop",
                table: "MStops",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isStop",
                table: "MStops");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "MStops",
                newName: "finish");

            migrationBuilder.AddColumn<int>(
                name: "minutes",
                table: "MStops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
