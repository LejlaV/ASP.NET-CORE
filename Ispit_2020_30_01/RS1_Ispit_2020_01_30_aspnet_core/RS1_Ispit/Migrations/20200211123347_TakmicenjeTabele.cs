using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class TakmicenjeTabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Takmicenje",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Razred = table.Column<int>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Zakljucano = table.Column<bool>(nullable: false),
                    PredmetID = table.Column<int>(nullable: false),
                    SkolaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicenje_Predmet_PredmetID",
                        column: x => x.PredmetID,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Takmicenje_Skola_SkolaId",
                        column: x => x.SkolaId,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TakmicenjeUcesnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bodovi = table.Column<int>(nullable: false),
                    Zakljucaj = table.Column<bool>(nullable: false),
                    TakmicenjeID = table.Column<int>(nullable: false),
                    OdjeljenjeStavkaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakmicenjeUcesnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeStavkaID",
                        column: x => x.OdjeljenjeStavkaID,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnik_Takmicenje_TakmicenjeID",
                        column: x => x.TakmicenjeID,
                        principalTable: "Takmicenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_PredmetID",
                table: "Takmicenje",
                column: "PredmetID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_SkolaId",
                table: "Takmicenje",
                column: "SkolaId");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeStavkaID",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeStavkaID");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_TakmicenjeID",
                table: "TakmicenjeUcesnik",
                column: "TakmicenjeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakmicenjeUcesnik");

            migrationBuilder.DropTable(
                name: "Takmicenje");
        }
    }
}
