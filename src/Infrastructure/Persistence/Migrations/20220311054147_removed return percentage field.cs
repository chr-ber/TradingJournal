using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class removedreturnpercentagefield : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropColumn(
          name: "ReturnPercentage",
          table: "Trades");

      migrationBuilder.DropColumn(
          name: "ReturnPercentage",
          table: "Executions");
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.AddColumn<decimal>(
          name: "ReturnPercentage",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          defaultValue: 0m);

      migrationBuilder.AddColumn<decimal>(
          name: "ReturnPercentage",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          defaultValue: 0m);
   }
}
