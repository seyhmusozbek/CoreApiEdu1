using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class counters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastValue = table.Column<int>(type: "int", nullable: false),
                    lastDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counters", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Counters");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2bb89fb4-4f4e-4d16-a62b-b2864cb9b604", "48927e15-3f14-40a3-a1b4-be23f41abedc", "ExtUser", "ExtUser" },
                    { "5c169c62-7d45-428a-9823-2f881948c34a", "a73931e0-d5c1-48ba-89bb-bd78616678ce", "LamUser", "LAMUSER" },
                    { "935b3485-c6b1-433a-bc8f-479569ca2d28", "f1285a18-5536-4cba-8ec9-ceb9c5125cf8", "FensUser", "FENSUSER" },
                    { "842683c9-3ba9-4d40-8ce6-fc8217a302c9", "ef8ee488-cd39-41f0-bec3-4b8d1bc7174a", "Stocker", "STOCKER" },
                    { "2af24115-2d49-4236-a134-720da22849ec", "40a2c83a-9dd6-42c5-8117-d1c6f1125781", "Administrator", "ADMINISTRATOR" },
                    { "f4041ba2-0343-430c-9e6d-a2382ab37d83", "3c16cb80-7597-48b6-a63f-526679369769", "Observer", "OBSERVER" },
                    { "f61f2a05-da55-48fa-8b8a-00f3c615b178", "1f10c9db-8fd0-41cd-92d3-748fee7a6990", "Planner", "PLANNER" }
                });
        }
    }
}
