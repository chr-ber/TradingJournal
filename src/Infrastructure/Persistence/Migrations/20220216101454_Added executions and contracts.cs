using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Server.Persistence.Migrations;

public partial class Addedexecutionsandcontracts : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {


      migrationBuilder.DropForeignKey(
          name: "FK_TradingAccount_Users_UserId",
          table: "TradingAccount");

      migrationBuilder.DropPrimaryKey(
          name: "PK_TradingAccount",
          table: "TradingAccount");

      migrationBuilder.RenameTable(
name: "TradingAccount",
newName: "TradingAccounts");

      migrationBuilder.RenameIndex(
          name: "IX_TradingAccount_UserId",
          table: "TradingAccounts",
          newName: "IX_TradingAccounts_UserId");


      migrationBuilder.AlterColumn<int>(
          name: "UserId",
          table: "TradingAccounts",
          type: "int",
          nullable: false,
          defaultValue: 0,
          oldClrType: typeof(int),
          oldType: "int",
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "TradingAccounts",
          type: "nvarchar(128)",
          maxLength: 128,
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(32)",
          oldMaxLength: 32);

      migrationBuilder.AddPrimaryKey(
          name: "PK_TradingAccounts",
          table: "TradingAccounts",
          column: "Id");

      migrationBuilder.CreateTable(
          name: "Contracts",
          columns: table => new
          {
             Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
             Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
          },
          constraints: table =>
          {
             table.PrimaryKey("PK_Contracts", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Trade",
          columns: table => new
          {
             Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
             Side = table.Column<int>(type: "int", nullable: false),
             ContractId = table.Column<int>(type: "int", nullable: true),
             TradingAccountId = table.Column<int>(type: "int", nullable: false),
             Size = table.Column<int>(type: "int", nullable: false),
             Position = table.Column<int>(type: "int", nullable: false),
             Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             AverageEntryPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             AverageExitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Return = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             ReturnPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             NetReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Result = table.Column<int>(type: "int", nullable: false),
             Confluences = table.Column<int>(type: "int", nullable: false),
             Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
             table.PrimaryKey("PK_Trade", x => x.Id);
             table.ForeignKey(
                    name: "FK_Trade_Contracts_ContractId",
                    column: x => x.ContractId,
                    principalTable: "Contracts",
                    principalColumn: "Id");
             table.ForeignKey(
                    name: "FK_Trade_TradingAccounts_TradingAccountId",
                    column: x => x.TradingAccountId,
                    principalTable: "TradingAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Executions",
          columns: table => new
          {
             Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
             Action = table.Column<int>(type: "int", nullable: false),
             TradeId = table.Column<int>(type: "int", nullable: false),
             ExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
             Size = table.Column<int>(type: "int", nullable: false),
             Position = table.Column<int>(type: "int", nullable: false),
             Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             Return = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             ReturnPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
             NetReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
          },
          constraints: table =>
          {
             table.PrimaryKey("PK_Executions", x => x.Id);
             table.ForeignKey(
                    name: "FK_Executions_Trade_TradeId",
                    column: x => x.TradeId,
                    principalTable: "Trade",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Executions_TradeId",
          table: "Executions",
          column: "TradeId");

      migrationBuilder.CreateIndex(
          name: "IX_Trade_ContractId",
          table: "Trade",
          column: "ContractId");

      migrationBuilder.CreateIndex(
          name: "IX_Trade_TradingAccountId",
          table: "Trade",
          column: "TradingAccountId");

      migrationBuilder.AddForeignKey(
          name: "FK_TradingAccounts_Users_UserId",
          table: "TradingAccounts",
          column: "UserId",
          principalTable: "Users",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropForeignKey(
          name: "FK_TradingAccounts_Users_UserId",
          table: "TradingAccounts");

      migrationBuilder.DropTable(
          name: "Executions");

      migrationBuilder.DropTable(
          name: "Trade");

      migrationBuilder.DropTable(
          name: "Contracts");

      migrationBuilder.DropPrimaryKey(
          name: "PK_TradingAccounts",
          table: "TradingAccounts");

      migrationBuilder.RenameTable(
          name: "TradingAccounts",
          newName: "TradingAccount");

      migrationBuilder.RenameIndex(
          name: "IX_TradingAccounts_UserId",
          table: "TradingAccount",
          newName: "IX_TradingAccount_UserId");

      migrationBuilder.AlterColumn<int>(
          name: "UserId",
          table: "TradingAccount",
          type: "int",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "TradingAccount",
          type: "nvarchar(32)",
          maxLength: 32,
          nullable: false,
          oldClrType: typeof(string),
          oldType: "nvarchar(128)",
          oldMaxLength: 128);

      migrationBuilder.AddPrimaryKey(
          name: "PK_TradingAccount",
          table: "TradingAccount",
          column: "Id");

      migrationBuilder.AddForeignKey(
          name: "FK_TradingAccount_Users_UserId",
          table: "TradingAccount",
          column: "UserId",
          principalTable: "Users",
          principalColumn: "Id");
   }
}
