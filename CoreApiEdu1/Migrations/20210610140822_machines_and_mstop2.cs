using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class machines_and_mstop2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    machineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    machineGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentStatus = table.Column<bool>(type: "bit", nullable: false),
                    currentOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MStops",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    machineid = table.Column<int>(type: "int", nullable: true),
                    start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    finish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    minutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MStops", x => x.id);
                    table.ForeignKey(
                        name: "FK_MStops_Machines_machineid",
                        column: x => x.machineid,
                        principalTable: "Machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MStops_machineid",
                table: "MStops",
                column: "machineid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MStops");

            migrationBuilder.DropTable(
                name: "Machines");
        }
    }
}
