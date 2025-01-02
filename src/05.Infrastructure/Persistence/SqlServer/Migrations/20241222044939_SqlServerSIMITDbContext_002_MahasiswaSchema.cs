using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    public partial class SqlServerSIMITDbContext_002_MahasiswaSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Accuracy",
                schema: "SIMIT",
                table: "Logbooks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "SIMIT",
                table: "Logbooks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "SIMIT",
                table: "Logbooks",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accuracy",
                schema: "SIMIT",
                table: "Logbooks");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "SIMIT",
                table: "Logbooks");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "SIMIT",
                table: "Logbooks");
        }
    }
}
