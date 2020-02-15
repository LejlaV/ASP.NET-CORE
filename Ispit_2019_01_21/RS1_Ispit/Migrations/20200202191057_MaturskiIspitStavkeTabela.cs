using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class MaturskiIspitStavkeTabela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaturskiIspitStavke",
                columns: table => new
                {
                    MaturskiIspitStavkeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojBodova = table.Column<int>(nullable: true),
                    MaturskiIspitID = table.Column<int>(nullable: false),
                    Ocjena = table.Column<int>(nullable: false),
                    OdjeljenjeStavkaID = table.Column<int>(nullable: false),
                    OdobrenPristup = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaturskiIspitStavke", x => x.MaturskiIspitStavkeID);
                    table.ForeignKey(
                        name: "FK_MaturskiIspitStavke_MaturskiIspit_MaturskiIspitID",
                        column: x => x.MaturskiIspitID,
                        principalTable: "MaturskiIspit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MaturskiIspitStavke_OdjeljenjeStavka_OdjeljenjeStavkaID",
                        column: x => x.OdjeljenjeStavkaID,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspitStavke_MaturskiIspitID",
                table: "MaturskiIspitStavke",
                column: "MaturskiIspitID");

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspitStavke_OdjeljenjeStavkaID",
                table: "MaturskiIspitStavke",
                column: "OdjeljenjeStavkaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaturskiIspitStavke");
        }
    }
}
