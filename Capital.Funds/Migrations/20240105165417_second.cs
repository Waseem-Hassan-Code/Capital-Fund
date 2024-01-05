using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2692d450-306a-45c0-994c-45a16c906a80");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "isPayable",
                table: "TenantPayments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "9af4e786-6060-4521-99b4-dd67d8ef5137", "admin@admin.com", "Male", false, "Capital Fund", "112233", "ISGkON/NvtV7+oS3uVntZgKXtW6SFrl8/liI59MB+/g=", "admin", "tHd4l6Soyc7u0UBVTlPUnLV/haezO4W6yhBpfdiZ0Ks=", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9af4e786-6060-4521-99b4-dd67d8ef5137");

            migrationBuilder.DropColumn(
                name: "isPayable",
                table: "TenantPayments");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "2692d450-306a-45c0-994c-45a16c906a80", "admin@admin.com", 1, false, "Capital Fund", "112233", "W2RwhPQLF7z+Y5qTnYvvC7XAGtMBV9XOVlnrwGY6RAQ=", "admin", "VSPGaGR585HoHRey4te4M9J7TwadF8YgK8hAbxV41C4=", false });
        }
    }
}
