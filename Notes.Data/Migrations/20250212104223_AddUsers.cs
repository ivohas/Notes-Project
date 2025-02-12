using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "513e8745-cb16-4553-961c-4d0eb93bc6c6", 0, "0d6160e5-8d39-49a3-9727-89e287605b94", "ApplicationUser", "test@mail.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEA+FkMqxjRXZptjXKI7tzPadJsvLCTcyjuWf+ocMTchldZo3WcM0AxMKMoGhFw25wg==", null, false, "c68fb1f4-30d0-4948-aebb-1905ea02a400", false, "test@mail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b4179065-ab48-40a5-bda9-9502ac423571", 0, "5e612ccb-5b75-4738-87e9-abadbd55a349", "ApplicationUser", "admin@mail.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEF2CEVSMqnkMXX1xK3W+WRrXPMn989r2Y2hPU4HMCEwwfLj92p2CWng+/JkksU2Pqw==", null, false, "98184373-4025-440a-88d8-ce063c795cbb", false, "admin@mail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "513e8745-cb16-4553-961c-4d0eb93bc6c6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4179065-ab48-40a5-bda9-9502ac423571");
        }
    }
}
