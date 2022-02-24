using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Server.Persistence.Migrations
{
    public partial class removedfieldsfromtradestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Contracts_ContractId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "Trades",
                newName: "SymbolId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_ContractId",
                table: "Trades",
                newName: "IX_Trades_SymbolId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "Trades",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Position",
                table: "Trades",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Contracts_SymbolId",
                table: "Trades",
                column: "SymbolId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Contracts_SymbolId",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "SymbolId",
                table: "Trades",
                newName: "ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_SymbolId",
                table: "Trades",
                newName: "IX_Trades_ContractId");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "Trades",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Trades",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Trades",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Trades",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Contracts_ContractId",
                table: "Trades",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}
