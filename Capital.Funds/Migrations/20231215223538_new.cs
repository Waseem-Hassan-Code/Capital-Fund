using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7af918f9-02cd-489a-9ac2-2d79c4b2ae85");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isEmailVerified",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "0f5bf085-ee73-442b-b62d-dcc16a28cb23", "admin@admin.com", 1, false, "Capital Fund", "112233", "CmARNJlN4uhOZh/CZo3KMdRzENjHKjJBGcBtS9eYEQs=", "admin", "J4slEC+WDFMyqtnTqMjDItg5BQqRpCB/Qql1qUsM12g=", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "0f5bf085-ee73-442b-b62d-dcc16a28cb23");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isEmailVerified",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "Name", "Password", "Role", "Salt" },
                values: new object[] { "7af918f9-02cd-489a-9ac2-2d79c4b2ae85", "admin@admin.com", 1, "Capital Fund", "0VdQ3re+oqkMGfBdBGkV4Xg6goD4KjmXiFjLTOdYgGE=", "admin", "5NLVzEUbIkM21/pa1qtYvo4YQ6oQ8TFXp4V3I/g89H8=" });
        }
    }
}
