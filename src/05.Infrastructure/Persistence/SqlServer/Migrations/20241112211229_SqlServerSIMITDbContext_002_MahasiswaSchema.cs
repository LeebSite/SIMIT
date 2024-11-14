﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    public partial class SqlServerSIMITDbContext_002_MahasiswaSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pembimbings",
                schema: "SIMIT",
                columns: table => new
                {
                    PembimbingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Nip = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Jabatan = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pembimbings", x => x.PembimbingId);
                });

            migrationBuilder.CreateTable(
                name: "Mahasiswas",
                schema: "SIMIT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Nim = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Kampus = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MulaiMagang = table.Column<DateTime>(type: "datetime", nullable: false),
                    SelesaiMagang = table.Column<DateTime>(type: "datetime", nullable: false),
                    Bagian = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PembimbingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mahasiswas_Pembimbings_PembimbingId",
                        column: x => x.PembimbingId,
                        principalSchema: "SIMIT",
                        principalTable: "Pembimbings",
                        principalColumn: "PembimbingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Laporans",
                schema: "SIMIT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileLaporan = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FileProject = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Deskripsi = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    MahasiswaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laporans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laporans_Mahasiswas_MahasiswaId",
                        column: x => x.MahasiswaId,
                        principalSchema: "SIMIT",
                        principalTable: "Mahasiswas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logbooks",
                schema: "SIMIT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogbookDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aktifitas = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    MahasiswaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logbooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logbooks_Mahasiswas_MahasiswaId",
                        column: x => x.MahasiswaId,
                        principalSchema: "SIMIT",
                        principalTable: "Mahasiswas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laporans_MahasiswaId",
                schema: "SIMIT",
                table: "Laporans",
                column: "MahasiswaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logbooks_MahasiswaId",
                schema: "SIMIT",
                table: "Logbooks",
                column: "MahasiswaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswas_PembimbingId",
                schema: "SIMIT",
                table: "Mahasiswas",
                column: "PembimbingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laporans",
                schema: "SIMIT");

            migrationBuilder.DropTable(
                name: "Logbooks",
                schema: "SIMIT");

            migrationBuilder.DropTable(
                name: "Mahasiswas",
                schema: "SIMIT");

            migrationBuilder.DropTable(
                name: "Pembimbings",
                schema: "SIMIT");
        }
    }
}
