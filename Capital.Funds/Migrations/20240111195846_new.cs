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
                keyValue: "2cade040-2547-4d5d-9490-9f94dba14bd5");

            migrationBuilder.AddColumn<int>(
                name: "RentPerMonth",
                table: "TenatDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ComplaintFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ComplaintId = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FileURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintFiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "9b0bb2fb-bdb5-4271-86ec-5375141cc0c1", "admin@admin.com", "Male", true, "Capital Fund", "112233", "52UymH5M8YdHNSYRx/8t2NvgdJ/qR5RAbZHdlDfpgAE=", "admin", "VPPGyZ3Q10Z0S5siELcdtbs8B9egCvAR8LWPWENAwvU=", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplaintFiles");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9b0bb2fb-bdb5-4271-86ec-5375141cc0c1");

            migrationBuilder.DropColumn(
                name: "RentPerMonth",
                table: "TenatDetails");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "2cade040-2547-4d5d-9490-9f94dba14bd5", "admin@admin.com", "Male", true, "Capital Fund", "112233", "FdBTS1x4IBH9hGvh8zlAlKGq7RS6a9+TYWt5sZJ3YoE=", "admin", "W5xFiEz4RIhCE05MTREyfS8LZmFYk5wusH8n3nXaW30=", true });
        }
    }
}
