using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class machine_colorfilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "colorFilter",
                table: "Machines",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "colorFilter",
                table: "Machines");

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
    }
}
