using Microsoft.EntityFrameworkCore.Migrations;

namespace _3DC.RecessWeekChallenge.Migrations._3DCRecessWeekChallenge
{
    public partial class AddFinalChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HackerrankFinalScore",
                table: "LeaderboardRow",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HackerrankTimeInt",
                table: "LeaderboardRow",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HackerrankFinalScore",
                table: "LeaderboardRow");

            migrationBuilder.DropColumn(
                name: "HackerrankTimeInt",
                table: "LeaderboardRow");
        }
    }
}
