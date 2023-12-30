using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class newTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "0f5bf085-ee73-442b-b62d-dcc16a28cb23");

            migrationBuilder.CreateTable(
                name: "PropertyDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    TypeofProperty = table.Column<string>(type: "TEXT", nullable: false),
                    NumberofBedrooms = table.Column<string>(type: "TEXT", nullable: false),
                    NumberofBathrooms = table.Column<string>(type: "TEXT", nullable: false),
                    isAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantComplaints",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    IsFixed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantComplaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", nullable: false),
                    Rent = table.Column<decimal>(type: "TEXT", nullable: false),
                    AreaMaintainienceFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    isLate = table.Column<bool>(type: "INTEGER", nullable: false),
                    LateFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    RentPayedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Month = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantPayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenatDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    PropertyId = table.Column<string>(type: "TEXT", nullable: false),
                    MovedIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MovedOut = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenatDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "2692d450-306a-45c0-994c-45a16c906a80", "admin@admin.com", 1, false, "Capital Fund", "112233", "W2RwhPQLF7z+Y5qTnYvvC7XAGtMBV9XOVlnrwGY6RAQ=", "admin", "VSPGaGR585HoHRey4te4M9J7TwadF8YgK8hAbxV41C4=", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyDetails");

            migrationBuilder.DropTable(
                name: "TenantComplaints");

            migrationBuilder.DropTable(
                name: "TenantPayments");

            migrationBuilder.DropTable(
                name: "TenatDetails");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2692d450-306a-45c0-994c-45a16c906a80");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "IsActive", "Name", "OTP", "Password", "Role", "Salt", "isEmailVerified" },
                values: new object[] { "0f5bf085-ee73-442b-b62d-dcc16a28cb23", "admin@admin.com", 1, false, "Capital Fund", "112233", "CmARNJlN4uhOZh/CZo3KMdRzENjHKjJBGcBtS9eYEQs=", "admin", "J4slEC+WDFMyqtnTqMjDItg5BQqRpCB/Qql1qUsM12g=", false });
        }
    }
}
