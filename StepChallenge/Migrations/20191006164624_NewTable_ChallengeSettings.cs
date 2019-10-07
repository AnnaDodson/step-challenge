using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StepChallenge.Migrations
{
    public partial class NewTable_ChallengeSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeSettings",
                columns: table => new
                {
                    ChallengeSettingsId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    DurationInWeeks = table.Column<int>(nullable: false),
                    ShowLeaderBoard = table.Column<bool>(nullable: false),
                    ShowLeaderBoardStepCounts = table.Column<bool>(nullable: false),
                    NumberOfParticipants = table.Column<int>(nullable: false),
                    NumberOfParticipantsInATeam = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSettings", x => x.ChallengeSettingsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeSettings");
        }
    }
}
