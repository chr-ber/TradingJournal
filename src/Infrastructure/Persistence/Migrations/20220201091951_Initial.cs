namespace TradingJournal.Infrastructure.Server.Persistence.Migrations;

public partial class Initial : Migration
{
   protected override void Up(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new
          {
             Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
             Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
             Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
             PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
             PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
          },
          constraints: table =>
          {
             table.PrimaryKey("PK_Users", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "TradingAccount",
          columns: table => new
          {
             Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
             Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
             Status = table.Column<int>(type: "int", nullable: false),
             UserId = table.Column<int>(type: "int", nullable: true),
             APIKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
             APISecret = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
          },
          constraints: table =>
          {
             table.PrimaryKey("PK_TradingAccount", x => x.Id);
             table.ForeignKey(
                    name: "FK_TradingAccount_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
          });

      migrationBuilder.CreateIndex(
          name: "IX_TradingAccount_UserId",
          table: "TradingAccount",
          column: "UserId");
   }

   protected override void Down(MigrationBuilder migrationBuilder)
   {
      migrationBuilder.DropTable(
          name: "TradingAccount");

      migrationBuilder.DropTable(
          name: "Users");
   }
}
