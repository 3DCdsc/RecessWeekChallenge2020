using Microsoft.EntityFrameworkCore.Migrations;

namespace _3DC.RecessWeekChallenge.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderboardRow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    HackerrankUsername = table.Column<string>(nullable: false),
                    LabScore = table.Column<int>(nullable: false),
                    HackerrankScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRow", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderboardRow");
        }
    }
}
