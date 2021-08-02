using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class stock_rserve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1015ed08-4e3b-4780-9d5e-670b04c07129");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f97a37a-deef-42ec-85c3-22214ea22fe2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39746d72-1b43-46b4-b3be-a78e17b2b5f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f3304d2-fe34-4608-a863-022d81e0caf1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8869e0a-a88c-4d82-94c3-cec3557b2fb3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dad3cac9-3cb6-4378-bc61-c03a98570632");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa345f43-0ac4-48f2-b3cd-67268b10d0b3");

            migrationBuilder.CreateTable(
                name: "StockReserves",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    orderNum = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    quantity1 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReserves", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockReserves");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f97a37a-deef-42ec-85c3-22214ea22fe2", "ef3b0733-51b4-4f5c-b63a-fb01e2181a92", "ExtUser", "ExtUser" },
                    { "39746d72-1b43-46b4-b3be-a78e17b2b5f0", "f811d580-44dc-4ce7-bc92-97f8d3bff7b1", "LamUser", "LAMUSER" },
                    { "dad3cac9-3cb6-4378-bc61-c03a98570632", "244aed09-a500-45cd-b861-49923523607a", "FensUser", "FENSUSER" },
                    { "1015ed08-4e3b-4780-9d5e-670b04c07129", "23a8dccc-078d-4d97-80f3-394f186a6ab8", "Stocker", "STOCKER" },
                    { "9f3304d2-fe34-4608-a863-022d81e0caf1", "eee70318-1843-4e36-ba3c-0f2231f89bff", "Administrator", "ADMINISTRATOR" },
                    { "c8869e0a-a88c-4d82-94c3-cec3557b2fb3", "6b2ecef2-9716-4d21-9615-3b2c874cd639", "Observer", "OBSERVER" },
                    { "fa345f43-0ac4-48f2-b3cd-67268b10d0b3", "97cb5046-008a-4c63-8f27-b588434bd3a7", "Planner", "PLANNER" }
                });
        }
    }
}
