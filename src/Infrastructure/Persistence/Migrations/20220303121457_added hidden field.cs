using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Server.Persistence.Migrations;

public partial class addedhiddenfield : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.AddColumn<bool>(
          name: "IsHidden",
          table: "Trades",
          type: "bit",
          nullable: false,
          defaultValue: false);
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropColumn(
          name: "IsHidden",
          table: "Trades");
   }
}
