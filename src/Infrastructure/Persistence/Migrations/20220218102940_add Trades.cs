using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class addTrades : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropForeignKey(
          name: "FK_Executions_Trade_TradeId",
          table: "Executions");

      migrationBuilder.DropForeignKey(
          name: "FK_Trade_Contracts_ContractId",
          table: "Trade");

      migrationBuilder.DropForeignKey(
          name: "FK_Trade_TradingAccounts_TradingAccountId",
          table: "Trade");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Trade",
          table: "Trade");

      migrationBuilder.RenameTable(
          name: "Trade",
          newName: "Trades");

      migrationBuilder.RenameIndex(
          name: "IX_Trade_TradingAccountId",
          table: "Trades",
          newName: "IX_Trades_TradingAccountId");

      migrationBuilder.RenameIndex(
          name: "IX_Trade_ContractId",
          table: "Trades",
          newName: "IX_Trades_ContractId");

      migrationBuilder.AddPrimaryKey(
          name: "PK_Trades",
          table: "Trades",
          column: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_Executions_Trades_TradeId",
          table: "Executions",
          column: "TradeId",
          principalTable: "Trades",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_Trades_Contracts_ContractId",
          table: "Trades",
          column: "ContractId",
          principalTable: "Contracts",
          principalColumn: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_Trades_TradingAccounts_TradingAccountId",
          table: "Trades",
          column: "TradingAccountId",
          principalTable: "TradingAccounts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropForeignKey(
          name: "FK_Executions_Trades_TradeId",
          table: "Executions");

      migrationBuilder.DropForeignKey(
          name: "FK_Trades_Contracts_ContractId",
          table: "Trades");

      migrationBuilder.DropForeignKey(
          name: "FK_Trades_TradingAccounts_TradingAccountId",
          table: "Trades");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Trades",
          table: "Trades");

      migrationBuilder.RenameTable(
          name: "Trades",
          newName: "Trade");

      migrationBuilder.RenameIndex(
          name: "IX_Trades_TradingAccountId",
          table: "Trade",
          newName: "IX_Trade_TradingAccountId");

      migrationBuilder.RenameIndex(
          name: "IX_Trades_ContractId",
          table: "Trade",
          newName: "IX_Trade_ContractId");

      migrationBuilder.AddPrimaryKey(
          name: "PK_Trade",
          table: "Trade",
          column: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_Executions_Trade_TradeId",
          table: "Executions",
          column: "TradeId",
          principalTable: "Trade",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_Trade_Contracts_ContractId",
          table: "Trade",
          column: "ContractId",
          principalTable: "Contracts",
          principalColumn: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_Trade_TradingAccounts_TradingAccountId",
          table: "Trade",
          column: "TradingAccountId",
          principalTable: "TradingAccounts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
   }
}
