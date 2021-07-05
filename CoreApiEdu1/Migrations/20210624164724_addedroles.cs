using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class addedroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "finishTime",
                table: "ChosenOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "ChosenOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "finishTime",
                table: "ChosenOrders");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "ChosenOrders");
        }
    }
}
