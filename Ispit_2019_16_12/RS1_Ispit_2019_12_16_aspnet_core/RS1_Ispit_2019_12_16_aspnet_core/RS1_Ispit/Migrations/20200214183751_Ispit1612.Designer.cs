﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RS1_Ispit_asp.net_core.EF;
using System;

namespace RS1_Ispit_asp.net_core.Migrations
{
    [DbContext(typeof(MojContext))]
    [Migration("20200214183751_Ispit1612")]
    partial class Ispit1612
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.DodjeljenPredmet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OdjeljenjeStavkaId");

                    b.Property<int>("PredmetId");

                    b.Property<int>("ZakljucnoKrajGodine");

                    b.HasKey("Id");

                    b.HasIndex("OdjeljenjeStavkaId");

                    b.HasIndex("PredmetId");

                    b.ToTable("DodjeljenPredmet");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ime");

                    b.Property<string>("Prezime");

                    b.Property<int>("SkolaID");

                    b.HasKey("Id");

                    b.HasIndex("SkolaID");

                    b.ToTable("Nastavnik");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Odjeljenje", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsPrebacenuViseOdjeljenje");

                    b.Property<string>("Oznaka");

                    b.Property<int>("Razred");

                    b.Property<int>("RazrednikID");

                    b.Property<int>("SkolaID");

                    b.Property<int>("SkolskaGodinaID");

                    b.HasKey("Id");

                    b.HasIndex("RazrednikID");

                    b.HasIndex("SkolaID");

                    b.HasIndex("SkolskaGodinaID");

                    b.ToTable("Odjeljenje");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.OdjeljenjeStavka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrojUDnevniku");

                    b.Property<int>("OdjeljenjeId");

                    b.Property<int>("UcenikId");

                    b.HasKey("Id");

                    b.HasIndex("OdjeljenjeId");

                    b.HasIndex("UcenikId");

                    b.ToTable("OdjeljenjeStavka");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PopravniIspit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Datum");

                    b.Property<int>("Nastavnik1ID");

                    b.Property<int>("Nastavnik2ID");

                    b.Property<int>("Nastavnik3ID");

                    b.Property<int>("PredmetID");

                    b.Property<int>("SkolaID");

                    b.Property<int>("SkolskaGodinaID");

                    b.HasKey("Id");

                    b.HasIndex("Nastavnik1ID");

                    b.HasIndex("Nastavnik2ID");

                    b.HasIndex("Nastavnik3ID");

                    b.HasIndex("PredmetID");

                    b.HasIndex("SkolaID");

                    b.HasIndex("SkolskaGodinaID");

                    b.ToTable("PopravniIspit");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PopravniIspitStavke", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Bodovi");

                    b.Property<int>("OdjeljenjeStavkaID");

                    b.Property<int>("PopravniIspitID");

                    b.Property<bool>("Pristup");

                    b.HasKey("Id");

                    b.HasIndex("OdjeljenjeStavkaID");

                    b.HasIndex("PopravniIspitID");

                    b.ToTable("PopravniIspitStavke");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PredajePredmet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NastavnikID");

                    b.Property<int>("OdjeljenjeID");

                    b.Property<int>("PredmetID");

                    b.HasKey("Id");

                    b.HasIndex("NastavnikID");

                    b.HasIndex("OdjeljenjeID");

                    b.HasIndex("PredmetID");

                    b.ToTable("PredajePredmet");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Predmet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv");

                    b.Property<int>("Razred");

                    b.HasKey("Id");

                    b.ToTable("Predmet");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Skola", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv");

                    b.HasKey("Id");

                    b.ToTable("Skola");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.SkolskaGodina", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Aktuelna");

                    b.Property<string>("Naziv");

                    b.HasKey("Id");

                    b.ToTable("SkolskaGodina");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Ucenik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImePrezime");

                    b.HasKey("Id");

                    b.ToTable("Ucenik");
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.DodjeljenPredmet", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.OdjeljenjeStavka", "OdjeljenjeStavka")
                        .WithMany()
                        .HasForeignKey("OdjeljenjeStavkaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Predmet", "Predmet")
                        .WithMany()
                        .HasForeignKey("PredmetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Skola", "Skola")
                        .WithMany()
                        .HasForeignKey("SkolaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.Odjeljenje", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", "Razrednik")
                        .WithMany()
                        .HasForeignKey("RazrednikID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Skola", "Skola")
                        .WithMany()
                        .HasForeignKey("SkolaID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.SkolskaGodina", "SkolskaGodina")
                        .WithMany()
                        .HasForeignKey("SkolskaGodinaID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.OdjeljenjeStavka", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Odjeljenje", "Odjeljenje")
                        .WithMany()
                        .HasForeignKey("OdjeljenjeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Ucenik", "Ucenik")
                        .WithMany()
                        .HasForeignKey("UcenikId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PopravniIspit", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", "Nastavnik1")
                        .WithMany()
                        .HasForeignKey("Nastavnik1ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", "Nastavnik2")
                        .WithMany()
                        .HasForeignKey("Nastavnik2ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", "Nastavnik3")
                        .WithMany()
                        .HasForeignKey("Nastavnik3ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Predmet", "Predmet")
                        .WithMany()
                        .HasForeignKey("PredmetID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Skola", "Skola")
                        .WithMany()
                        .HasForeignKey("SkolaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.SkolskaGodina", "SkolskaGodina")
                        .WithMany()
                        .HasForeignKey("SkolskaGodinaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PopravniIspitStavke", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.OdjeljenjeStavka", "OdjeljenjeStavka")
                        .WithMany()
                        .HasForeignKey("OdjeljenjeStavkaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.PopravniIspit", "PopravniIspit")
                        .WithMany()
                        .HasForeignKey("PopravniIspitID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS1_Ispit_asp.net_core.EntityModels.PredajePredmet", b =>
                {
                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Nastavnik", "Nastavnik")
                        .WithMany()
                        .HasForeignKey("NastavnikID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Odjeljenje", "Odjeljenje")
                        .WithMany()
                        .HasForeignKey("OdjeljenjeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RS1_Ispit_asp.net_core.EntityModels.Predmet", "Predmet")
                        .WithMany()
                        .HasForeignKey("PredmetID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
