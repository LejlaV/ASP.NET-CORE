using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class izmjena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zakljucaj",
                table: "TakmicenjeUcesnik",
                newName: "Pristupio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pristupio",
                table: "TakmicenjeUcesnik",
                newName: "Zakljucaj");
        }
    }
}
