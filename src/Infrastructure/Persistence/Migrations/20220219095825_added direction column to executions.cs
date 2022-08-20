using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations;

public partial class addeddirectioncolumntoexecutions : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.AddColumn<int>(
          name: "Direction",
          table: "Executions",
          type: "int",
          nullable: false,
          defaultValue: 0);
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropColumn(
          name: "Direction",
          table: "Executions");
   }
}
