using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class plan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "483afdf8-8a46-4172-9cac-23a11db07353");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5648d6a0-f660-49e3-a74a-b9b23b67995c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e038f44-cc2f-4954-900b-53a7e9f160fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7baef34-d7c4-4bd7-b45a-0bc1da6a8734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac42ae64-7fd0-40c4-b3d4-09b133954ab3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7c050d2-3c42-4037-a43e-d9a524b87e03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1af3893-96bd-49e9-89b9-39852b02b0ea");

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    machine = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    caliber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    finishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hour = table.Column<double>(type: "float", nullable: false),
                    quantity1 = table.Column<double>(type: "float", nullable: false),
                    readyQuantity = table.Column<double>(type: "float", nullable: false),
                    orderNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    customerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "062067f0-700d-4e44-bdb8-3a80b0eb5934", "a7078136-163f-4366-91f5-e84c2def72b0", "ExtUser", "ExtUser" },
                    { "1a26a5b2-1e1a-409c-9da9-ec87fd6d7bc0", "638f6bfc-003e-4605-b200-21f355f81bef", "LamUser", "LAMUSER" },
                    { "67e970dc-884b-452f-ad1f-a37260f4f8d1", "12dc7510-9818-41b1-9d44-24cbebf8a14d", "FensUser", "FENSUSER" },
                    { "db245fa0-3b70-4760-9c23-02d11fc537ba", "28a84367-ca86-4f13-853b-daa96c7ff4c0", "Stocker", "STOCKER" },
                    { "333a9e23-b703-4e9d-9876-8817182218c2", "4a507ee2-8f3d-4e38-8239-750dae977db6", "Administrator", "ADMINISTRATOR" },
                    { "ab030f7e-0dff-4931-8594-694a70568286", "5b2b664b-3051-46c1-8742-454dbe67f6a8", "Observer", "OBSERVER" },
                    { "5a98448a-7d44-4882-bf73-710e231cec7b", "078469d4-07ae-4944-890b-13ea7a811249", "Planner", "PLANNER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "062067f0-700d-4e44-bdb8-3a80b0eb5934");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a26a5b2-1e1a-409c-9da9-ec87fd6d7bc0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "333a9e23-b703-4e9d-9876-8817182218c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a98448a-7d44-4882-bf73-710e231cec7b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67e970dc-884b-452f-ad1f-a37260f4f8d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab030f7e-0dff-4931-8594-694a70568286");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db245fa0-3b70-4760-9c23-02d11fc537ba");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b7c050d2-3c42-4037-a43e-d9a524b87e03", "4ea8b216-58e5-43a2-a3e3-fc8cd961656e", "ExtUser", "ExtUser" },
                    { "5648d6a0-f660-49e3-a74a-b9b23b67995c", "73429dbe-1b49-4c49-9955-ed5ca0764037", "LamUser", "LAMUSER" },
                    { "483afdf8-8a46-4172-9cac-23a11db07353", "1176125f-9631-4f2e-8e5b-7275804009dd", "FensUser", "FENSUSER" },
                    { "a7baef34-d7c4-4bd7-b45a-0bc1da6a8734", "e42a4f5e-d1cf-416b-b828-a5b3613bdff2", "Stocker", "STOCKER" },
                    { "ac42ae64-7fd0-40c4-b3d4-09b133954ab3", "5362838d-8968-4197-8b51-22581b6cec05", "Administrator", "ADMINISTRATOR" },
                    { "8e038f44-cc2f-4954-900b-53a7e9f160fb", "c7305abb-2d58-4190-9468-f9d8a6502252", "Observer", "OBSERVER" },
                    { "e1af3893-96bd-49e9-89b9-39852b02b0ea", "92b026a5-6a3e-4080-9e39-3c1de5f1c19c", "Planner", "PLANNER" }
                });
        }
    }
}
