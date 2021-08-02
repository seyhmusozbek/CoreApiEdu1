using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class whtransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WHTransfers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stokKodu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cikDepo = table.Column<int>(type: "int", nullable: false),
                    girDepo = table.Column<int>(type: "int", nullable: false),
                    miktar = table.Column<double>(type: "float", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    obr = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHTransfers", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WHTransfers");
        }
    }
}
