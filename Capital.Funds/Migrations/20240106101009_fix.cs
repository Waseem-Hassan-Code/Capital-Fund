using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9af4e786-6060-4521-99b4-dd67d8ef5137");

            migrationBuilder.AlterColumn<int>(
                name: "NumberofBedrooms",
                table: "PropertyDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "NumberofBathrooms",
                table: "PropertyDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "96bcd345-42d7-4485-ba02-9dda436f475c", "admin@admin.com", "Male", true, "Capital Fund", "112233", "AIDTUerVWeC7Li2bVfJ3NttjN91Tm3tp/Vq5j4uRV+k=", "admin", "8YFJDNwcRuZUlzTQvUaUPbTCUDVxymOBS5cHjiO91mA=", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "96bcd345-42d7-4485-ba02-9dda436f475c");

            migrationBuilder.AlterColumn<string>(
                name: "NumberofBedrooms",
                table: "PropertyDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "NumberofBathrooms",
                table: "PropertyDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "9af4e786-6060-4521-99b4-dd67d8ef5137", "admin@admin.com", "Male", false, "Capital Fund", "112233", "ISGkON/NvtV7+oS3uVntZgKXtW6SFrl8/liI59MB+/g=", "admin", "tHd4l6Soyc7u0UBVTlPUnLV/haezO4W6yhBpfdiZ0Ks=", false });
        }
    }
}
