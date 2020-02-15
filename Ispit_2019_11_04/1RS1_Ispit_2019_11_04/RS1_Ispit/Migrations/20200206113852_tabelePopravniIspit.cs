using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class tabelePopravniIspit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PopravniIspit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    PredmetID = table.Column<int>(nullable: false),
                    SkolaID = table.Column<int>(nullable: false),
                    SkolskaGodinaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopravniIspit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopravniIspit_Predmet_PredmetID",
                        column: x => x.PredmetID,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PopravniIspit_Skola_SkolaID",
                        column: x => x.SkolaID,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PopravniIspit_SkolskaGodina_SkolskaGodinaID",
                        column: x => x.SkolskaGodinaID,
                        principalTable: "SkolskaGodina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PopravniIspitStavke",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bodovi = table.Column<int>(nullable: true),
                    OdjeljenjeStavkaID = table.Column<int>(nullable: false),
                    PopravniIspitID = table.Column<int>(nullable: false),
                    Prisutan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopravniIspitStavke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopravniIspitStavke_OdjeljenjeStavka_OdjeljenjeStavkaID",
                        column: x => x.OdjeljenjeStavkaID,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PopravniIspitStavke_PopravniIspit_PopravniIspitID",
                        column: x => x.PopravniIspitID,
                        principalTable: "PopravniIspit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspit_PredmetID",
                table: "PopravniIspit",
                column: "PredmetID");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspit_SkolaID",
                table: "PopravniIspit",
                column: "SkolaID");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspit_SkolskaGodinaID",
                table: "PopravniIspit",
                column: "SkolskaGodinaID");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitStavke_OdjeljenjeStavkaID",
                table: "PopravniIspitStavke",
                column: "OdjeljenjeStavkaID");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitStavke_PopravniIspitID",
                table: "PopravniIspitStavke",
                column: "PopravniIspitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PopravniIspitStavke");

            migrationBuilder.DropTable(
                name: "PopravniIspit");
        }
    }
}
