using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiEdu1.Migrations
{
    public partial class role_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "0dd87781-5ffe-4093-b53d-e454190f3d9c");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "480b121b-66a5-4d0f-8f0a-3d98ebcd6ee6");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "78630904-032c-4858-b1d9-21e8607ecfda");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "89010881-fcc4-45d5-bff5-bda5f64107a6");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "9a53e720-4a1a-46da-8aad-01b5b53c18e6");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "9a9ffb80-2a51-421d-92ee-4b074fa3565f");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "bcf64e62-e7bf-4d7b-8ae3-61a30b4d6aa4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    //{ "b49782cf-012e-41da-a0e4-0abd83486d76", "96038315-3e5e-4efd-96c1-b45f839f2b07", "ExtUser", "ExtUser" },
                    //{ "745821f7-fed3-48f7-9acb-4ecc541005e2", "40552884-9dcb-47e4-bb4b-d48bc7d8991b", "LamUser", "LAMUSER" },
                    //{ "7a99f5ed-e864-4abe-9ce6-d0c294df4c86", "030e02f2-62da-4cf8-aad0-ad6cd242d722", "FensUser", "FENSUSER" },
                    //{ "06c0ba0f-d983-4570-b7ee-1152c15a9c8b", "00a79955-591e-4b02-ab24-913b22748e59", "Stocker", "STOCKER" },
                    //{ "859bfbc4-5d76-4b7d-b773-a373d3e93171", "09909777-a080-4001-b546-8fb715945eea", "Administrator", "ADMINISTRATOR" },
                    //{ "2583063e-2c66-433c-8632-1d404026321e", "2cde0515-f565-44df-853f-2d2888ee9658", "Observer", "OBSERVER" },
                    //{ "8e64a5ea-d78b-4497-a942-2975402ba579", "d3376caa-1b23-4c9b-a07c-34965e4ddc75", "Planner", "PLANNER" },
                    { "31202c72-1c78-4eea-8d96-572838b96fc0", "1b3437c9-3a01-40e7-81f8-8f4f922ac06c", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06c0ba0f-d983-4570-b7ee-1152c15a9c8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2583063e-2c66-433c-8632-1d404026321e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31202c72-1c78-4eea-8d96-572838b96fc0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "745821f7-fed3-48f7-9acb-4ecc541005e2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a99f5ed-e864-4abe-9ce6-d0c294df4c86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "859bfbc4-5d76-4b7d-b773-a373d3e93171");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e64a5ea-d78b-4497-a942-2975402ba579");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b49782cf-012e-41da-a0e4-0abd83486d76");

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
    }
}
