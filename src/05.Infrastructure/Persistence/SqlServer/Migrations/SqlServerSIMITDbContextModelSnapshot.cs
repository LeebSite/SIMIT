﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pertamina.SIMIT.Infrastructure.Persistence.SqlServer;

#nullable disable

namespace Pertamina.SIMIT.Infrastructure.Persistence.SqlServer.Migrations
{
    [DbContext(typeof(SqlServerSIMITDbContext))]
    partial class SqlServerSIMITDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ClientApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FromIpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Audits", "SIMIT");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Laporan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Deskripsi")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("FileLaporan")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileProject")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("MahasiswaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("MahasiswaId")
                        .IsUnique();

                    b.ToTable("Laporans", "SIMIT");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Logbook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aktifitas")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LogbookDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MahasiswaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("MahasiswaId");

                    b.ToTable("Logbooks", "SIMIT");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Mahasiswa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bagian")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Kampus")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("MulaiMagang")
                        .HasColumnType("datetime");

                    b.Property<string>("Nama")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nim")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("PembimbingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SelesaiMagang")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("PembimbingId");

                    b.ToTable("Mahasiswas", "SIMIT");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Pembimbing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Jabatan")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nama")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nip")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Pembimbings", "SIMIT");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Audit", b =>
                {
                    b.OwnsOne("Pertamina.SIMIT.Base.ValueObjects.Geolocation", "FromGeolocation", b1 =>
                        {
                            b1.Property<Guid>("AuditId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Accuracy")
                                .HasColumnType("float")
                                .HasColumnName("Accuracy");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("AuditId");

                            b1.ToTable("Audits", "SIMIT");

                            b1.WithOwner()
                                .HasForeignKey("AuditId");
                        });

                    b.Navigation("FromGeolocation");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Laporan", b =>
                {
                    b.HasOne("Pertamina.SIMIT.Domain.Entities.Mahasiswa", "Mahasiswa")
                        .WithOne("Laporan")
                        .HasForeignKey("Pertamina.SIMIT.Domain.Entities.Laporan", "MahasiswaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mahasiswa");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Logbook", b =>
                {
                    b.HasOne("Pertamina.SIMIT.Domain.Entities.Mahasiswa", "Mahasiswa")
                        .WithMany("Logbooks")
                        .HasForeignKey("MahasiswaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mahasiswa");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Mahasiswa", b =>
                {
                    b.HasOne("Pertamina.SIMIT.Domain.Entities.Pembimbing", "Pembimbing")
                        .WithMany("Mahasiswas")
                        .HasForeignKey("PembimbingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pembimbing");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Mahasiswa", b =>
                {
                    b.Navigation("Laporan")
                        .IsRequired();

                    b.Navigation("Logbooks");
                });

            modelBuilder.Entity("Pertamina.SIMIT.Domain.Entities.Pembimbing", b =>
                {
                    b.Navigation("Mahasiswas");
                });
#pragma warning restore 612, 618
        }
    }
}
