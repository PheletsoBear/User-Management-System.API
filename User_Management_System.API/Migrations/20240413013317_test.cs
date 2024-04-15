using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.API.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityUser",
                keyColumn: "Id",
                keyValue: "0ec5a695-2b99-413c-9fbb-3fb825961a42",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a72a1f99-aeef-42d9-a84f-5d5bb2d68164", "AQAAAAIAAYagAAAAEPySf2XSjdhQ5TNqAOU4AO4ztkDZHA8mISDLpojt4LXEwxR55xqN9zINX6T43q4a3w==", "9b377011-3d37-4db2-9dae-a83a1e936bf6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityUser",
                keyColumn: "Id",
                keyValue: "0ec5a695-2b99-413c-9fbb-3fb825961a42",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c6864ae-2a41-4e27-ab94-92ddcaba0db3", "AQAAAAIAAYagAAAAEISt/ZWdlA37BRjkA1gToURhYXVev6LeygTVa0Q86i9Ip70BrM5XCBRuIQ5NUGBSpA==", "00c2b213-bb1f-496e-af10-f1c246272b0d" });
        }
    }
}
