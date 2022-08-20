using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class updatedusertable : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.RenameColumn(
          name: "Username",
          table: "Users",
          newName: "DisplayName");
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.RenameColumn(
          name: "DisplayName",
          table: "Users",
          newName: "Username");
   }
}
