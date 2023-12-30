using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capital.Funds.Migrations
{
    /// <inheritdoc />
    public partial class FirstOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Salt = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Gender", "Name", "Password", "Role", "Salt" },
                values: new object[] { "7af918f9-02cd-489a-9ac2-2d79c4b2ae85", "admin@admin.com", 1, "Capital Fund", "0VdQ3re+oqkMGfBdBGkV4Xg6goD4KjmXiFjLTOdYgGE=", "admin", "5NLVzEUbIkM21/pa1qtYvo4YQ6oQ8TFXp4V3I/g89H8=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
