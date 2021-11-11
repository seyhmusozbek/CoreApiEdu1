using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class counting_tables_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CountMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    exp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warehouse = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountMaster", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CountDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stokKodu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    miktar = table.Column<double>(type: "float", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    obr = table.Column<int>(type: "int", nullable: false),
                    countMasterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_CountDetails_CountMaster_countMasterId",
                        column: x => x.countMasterId,
                        principalTable: "CountMaster",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

         

            migrationBuilder.CreateIndex(
                name: "IX_CountDetails_countMasterId",
                table: "CountDetails",
                column: "countMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountDetails");

            migrationBuilder.DropTable(
                name: "CountMaster");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0576c491-c73c-4af6-8fef-cf82a39d80af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29f49833-aa53-4af6-8746-4b6ca5aec6d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37356287-fe00-4295-8bd7-1abf1d2e6041");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d1662aa-2847-40a0-98ca-696acf5dd0c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7071d806-d290-4220-bde4-68d28ebc0bcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96503ed7-f1d3-4ee6-a8cf-846b490da0bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5bc4407-c56d-4b9d-97f2-413be403dcca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4db9cc1-d80c-4749-9745-3e6fbb37e08e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2690fc6a-b40c-440b-964a-5d5f6ef70611", "3f2648ae-d267-4a08-864d-97e7f591e4b4", "ExtUser", "ExtUser" },
                    { "73bac9d0-39e3-4eb2-b61c-3c6fefd76e0d", "d2a7a308-196d-4a6a-b2c0-d0d2cea3bd5c", "LamUser", "LAMUSER" },
                    { "1d59ce39-9697-4837-93f5-71b5c75d13a6", "b463ab45-709e-4702-ad86-ef1c9d5600d5", "FensUser", "FENSUSER" },
                    { "ed38c7de-3a74-4e68-940a-5a4869760b81", "b17b856a-a5bd-4d7f-bfa4-9af338a2c768", "Stocker", "STOCKER" },
                    { "24bc7d72-d16b-4069-80f9-403e6f5392f2", "5782930f-cacb-4c6d-a689-e2b872907e07", "Administrator", "ADMINISTRATOR" },
                    { "4e440c48-4482-4740-b8d3-8c55e1e6eb02", "fe0875e6-02b6-44d8-8a22-e6e4bd3eb046", "Observer", "OBSERVER" },
                    { "39656913-46d4-4f90-aa0d-f9f6753067a9", "d976b228-2218-40c4-b197-2114c7dc5711", "Planner", "PLANNER" },
                    { "b4a7e1b4-973f-4e65-a2f8-cfcb9687b012", "9fb2271d-66c7-496a-9d46-70bb2709ffb0", "User", "USER" }
                });
        }
    }
}
