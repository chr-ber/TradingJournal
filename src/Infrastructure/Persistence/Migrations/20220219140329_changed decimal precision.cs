using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class changeddecimalprecision : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.AlterColumn<decimal>(
          name: "Size",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "ReturnPercentage",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Return",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Position",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "NetReturn",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Cost",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "AverageExitPrice",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "AverageEntryPrice",
          table: "Trades",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Value",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Size",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "ReturnPercentage",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Return",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Price",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Position",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "NetReturn",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");

      migrationBuilder.AlterColumn<decimal>(
          name: "Fee",
          table: "Executions",
          type: "decimal(18,6)",
          precision: 18,
          scale: 6,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,2)");
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.AlterColumn<decimal>(
          name: "Size",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "ReturnPercentage",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Return",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Position",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "NetReturn",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Cost",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "AverageExitPrice",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "AverageEntryPrice",
          table: "Trades",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Value",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Size",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "ReturnPercentage",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Return",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Price",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Position",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "NetReturn",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);

      migrationBuilder.AlterColumn<decimal>(
          name: "Fee",
          table: "Executions",
          type: "decimal(18,2)",
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(18,6)",
          oldPrecision: 18,
          oldScale: 6);
   }
}
