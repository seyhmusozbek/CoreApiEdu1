using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class mstop_laststop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isStop",
                table: "MStops");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "MStops",
                newName: "finish");

            migrationBuilder.AddColumn<DateTime>(
                name: "lastStopped",
                table: "Machines",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastStopped",
                table: "Machines");

            migrationBuilder.RenameColumn(
                name: "finish",
                table: "MStops",
                newName: "start");

            migrationBuilder.AddColumn<bool>(
                name: "isStop",
                table: "MStops",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
