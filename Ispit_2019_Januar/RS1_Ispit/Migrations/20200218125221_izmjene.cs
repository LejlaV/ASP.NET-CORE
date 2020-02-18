using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class izmjene : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Bodovi",
                table: "MaturskiIspitStavke",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Bodovi",
                table: "MaturskiIspitStavke",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
