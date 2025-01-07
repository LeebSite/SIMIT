using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    public partial class SqlServerSIMITDbContext_003_SimitSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "SIMIT",
                table: "Pembimbings",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "SIMIT",
                table: "Pembimbings");
        }
    }
}
