using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class roles_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0dd87781-5ffe-4093-b53d-e454190f3d9c", "639306e0-5119-4011-9f06-310b485c12b3", "ExtUser", "ExtUser" },
                    { "bcf64e62-e7bf-4d7b-8ae3-61a30b4d6aa4", "b9b04b38-11f9-4abd-be2a-3379b0ae2597", "LamUser", "LAMUSER" },
                    { "78630904-032c-4858-b1d9-21e8607ecfda", "b7161fff-8159-4530-9a15-cbe9d46ad1ca", "FensUser", "FENSUSER" },
                    { "9a9ffb80-2a51-421d-92ee-4b074fa3565f", "06da780e-cd2a-4a9e-80d5-c318691044b0", "Stocker", "STOCKER" },
                    { "480b121b-66a5-4d0f-8f0a-3d98ebcd6ee6", "22111eb5-9b93-44ea-8667-496f5ffee68f", "Administrator", "ADMINISTRATOR" },
                    { "9a53e720-4a1a-46da-8aad-01b5b53c18e6", "6b742249-df59-4f99-9744-38fd7d006f26", "Observer", "OBSERVER" },
                    { "89010881-fcc4-45d5-bff5-bda5f64107a6", "f3317f6e-a509-4216-b0bb-41687f48536f", "Planner", "PLANNER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0dd87781-5ffe-4093-b53d-e454190f3d9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "480b121b-66a5-4d0f-8f0a-3d98ebcd6ee6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78630904-032c-4858-b1d9-21e8607ecfda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89010881-fcc4-45d5-bff5-bda5f64107a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a53e720-4a1a-46da-8aad-01b5b53c18e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a9ffb80-2a51-421d-92ee-4b074fa3565f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bcf64e62-e7bf-4d7b-8ae3-61a30b4d6aa4");
        }
    }
}
