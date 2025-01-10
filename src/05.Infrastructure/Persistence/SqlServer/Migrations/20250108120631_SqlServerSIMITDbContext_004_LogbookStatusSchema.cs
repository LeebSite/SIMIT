using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    public partial class SqlServerSIMITDbContext_004_LogbookStatusSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approval",
                schema: "SIMIT",
                table: "Logbooks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval",
                schema: "SIMIT",
                table: "Logbooks");
        }
    }
}
