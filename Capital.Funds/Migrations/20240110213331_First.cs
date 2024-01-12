using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1e8f8220-2ee8-4241-9647-916482206cb1");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "2cade040-2547-4d5d-9490-9f94dba14bd5", "admin@admin.com", "Male", true, "Capital Fund", "112233", "FdBTS1x4IBH9hGvh8zlAlKGq7RS6a9+TYWt5sZJ3YoE=", "admin", "W5xFiEz4RIhCE05MTREyfS8LZmFYk5wusH8n3nXaW30=", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2cade040-2547-4d5d-9490-9f94dba14bd5");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "1e8f8220-2ee8-4241-9647-916482206cb1", "admin@admin.com", "Male", true, "Capital Fund", "112233", "VUct4jIlYf5wizQWudIuGblzgBNCLgEwGj6CLHH71o4=", "admin", "1CtsbaD8pa2uZqIrrgqd4vfjO8cu84vTtZki2FlIi/U=", true });
        }
    }
}
