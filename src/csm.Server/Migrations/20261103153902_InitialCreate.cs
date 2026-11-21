using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csm.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    Puuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummonerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummonerLevel = table.Column<int>(type: "int", nullable: false),
                    RiotIdGameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiotIdTagLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileIcon = table.Column<int>(type: "int", nullable: false),
                    ChampionId = table.Column<int>(type: "int", nullable: false),
                    ChampionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChampionLevel = table.Column<int>(type: "int", nullable: false),
                    ChampionExperience = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TeamPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndividualPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    Win = table.Column<bool>(type: "bit", nullable: false),
                    GoldEarned = table.Column<int>(type: "int", nullable: false),
                    GoldSpent = table.Column<int>(type: "int", nullable: false),
                    TotalMinionsKilled = table.Column<int>(type: "int", nullable: false),
                    neutralMinionsKilled = table.Column<int>(type: "int", nullable: false),
                    TotalDamageDealt = table.Column<int>(type: "int", nullable: false),
                    TotalDamageDealtToChampions = table.Column<int>(type: "int", nullable: false),
                    TotalDamageTaken = table.Column<int>(type: "int", nullable: false),
                    DamageSelfMitigated = table.Column<int>(type: "int", nullable: false),
                    TotalHeal = table.Column<int>(type: "int", nullable: false),
                    VisionScore = table.Column<int>(type: "int", nullable: false),
                    WardsPlaced = table.Column<int>(type: "int", nullable: false),
                    wardsKilled = table.Column<int>(type: "int", nullable: false),
                    Item0 = table.Column<int>(type: "int", nullable: false),
                    Item1 = table.Column<int>(type: "int", nullable: false),
                    Item2 = table.Column<int>(type: "int", nullable: false),
                    Item3 = table.Column<int>(type: "int", nullable: false),
                    Item4 = table.Column<int>(type: "int", nullable: false),
                    Item5 = table.Column<int>(type: "int", nullable: false),
                    Item6 = table.Column<int>(type: "int", nullable: false),
                    SummonerSpell1Id = table.Column<int>(type: "int", nullable: false),
                    SummonerSpell2Id = table.Column<int>(type: "int", nullable: false),
                    FirstBloodKill = table.Column<bool>(type: "bit", nullable: false),
                    FirstBloodAssist = table.Column<bool>(type: "bit", nullable: false),
                    FirstTowerKill = table.Column<bool>(type: "bit", nullable: false),
                    FirstTowerAssist = table.Column<bool>(type: "bit", nullable: false),
                    DoubleKills = table.Column<int>(type: "int", nullable: false),
                    TripleKills = table.Column<int>(type: "int", nullable: false),
                    QuadraKills = table.Column<int>(type: "int", nullable: false),
                    PentaKills = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
