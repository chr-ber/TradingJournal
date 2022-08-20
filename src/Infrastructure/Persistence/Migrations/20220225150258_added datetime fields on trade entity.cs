using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class addeddatetimefieldsontradeentity : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.RenameColumn(
          name: "Result",
          table: "Trades",
          newName: "Status");

      migrationBuilder.AddColumn<DateTime>(
          name: "ClosedAt",
          table: "Trades",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

      migrationBuilder.AddColumn<DateTime>(
          name: "OpenedAt",
          table: "Trades",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropColumn(
          name: "ClosedAt",
          table: "Trades");

      migrationBuilder.DropColumn(
          name: "OpenedAt",
          table: "Trades");

      migrationBuilder.RenameColumn(
          name: "Status",
          table: "Trades",
          newName: "Result");
   }
}
