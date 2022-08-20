using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class changedsizeandpositiontodecimal : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropForeignKey(
          name: "FK_Trades_Contracts_SymbolId",
          table: "Trades");

      migrationBuilder.AlterColumn<int>(
          name: "SymbolId",
          table: "Trades",
          type: "int",
          nullable: false,
          defaultValue: 0,
          oldClrType: typeof(int),
          oldType: "int",
          oldNullable: true);

      migrationBuilder.AlterColumn<decimal>(
          name: "Size",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<decimal>(
          name: "Position",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AddForeignKey(
          name: "FK_Trades_Contracts_SymbolId",
          table: "Trades",
          column: "SymbolId",
          principalTable: "Contracts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropForeignKey(
          name: "FK_Trades_Contracts_SymbolId",
          table: "Trades");

      migrationBuilder.AlterColumn<int>(
          name: "SymbolId",
          table: "Trades",
          type: "int",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<int>(
          name: "Size",
          table: "Executions",
          type: "int",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<int>(
          name: "Position",
          table: "Executions",
          type: "int",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AddForeignKey(
          name: "FK_Trades_Contracts_SymbolId",
          table: "Trades",
          column: "SymbolId",
          principalTable: "Contracts",
          principalColumn: "Id");
   }
}
