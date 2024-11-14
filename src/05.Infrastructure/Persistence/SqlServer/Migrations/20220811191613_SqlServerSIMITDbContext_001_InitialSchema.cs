using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    public partial class SqlServerSIMITDbContext_001_InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SIMIT");

            migrationBuilder.CreateTable(
                name: "Audits",
                schema: "SIMIT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientApplicationId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FromIpAddress = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Accuracy = table.Column<double>(type: "float", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits",
                schema: "SIMIT");
        }
    }
}
