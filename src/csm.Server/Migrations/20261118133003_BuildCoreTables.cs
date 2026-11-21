using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csm.Server.Migrations
{
    /// <inheritdoc />
    public partial class BuildCoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "TeamPosition",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "wardsKilled",
                table: "Participants",
                newName: "WardsKilled");

            migrationBuilder.CreateTable(
                name: "Builds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChampionId = table.Column<int>(type: "int", nullable: false),
                    ChampionIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Item0 = table.Column<int>(type: "int", nullable: false),
                    Item1 = table.Column<int>(type: "int", nullable: false),
                    Item2 = table.Column<int>(type: "int", nullable: false),
                    Item3 = table.Column<int>(type: "int", nullable: false),
                    Item4 = table.Column<int>(type: "int", nullable: false),
                    Item5 = table.Column<int>(type: "int", nullable: false),
                    Item6 = table.Column<int>(type: "int", nullable: false),
                    PrimaryStyle = table.Column<int>(type: "int", nullable: false),
                    Perk1 = table.Column<int>(type: "int", nullable: false),
                    Perk2 = table.Column<int>(type: "int", nullable: false),
                    Perk3 = table.Column<int>(type: "int", nullable: false),
                    Perk4 = table.Column<int>(type: "int", nullable: false),
                    SecondaryStyle = table.Column<int>(type: "int", nullable: false),
                    SubPerk1 = table.Column<int>(type: "int", nullable: false),
                    SubPerk2 = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Flex = table.Column<int>(type: "int", nullable: false),
                    Offense = table.Column<int>(type: "int", nullable: false),
                    Spell1 = table.Column<int>(type: "int", nullable: false),
                    Spell2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Builds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChampionId = table.Column<int>(type: "int", nullable: false),
                    ChampionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Item0 = table.Column<int>(type: "int", nullable: false),
                    Item1 = table.Column<int>(type: "int", nullable: false),
                    Item2 = table.Column<int>(type: "int", nullable: false),
                    Spell1 = table.Column<int>(type: "int", nullable: false),
                    Spell2 = table.Column<int>(type: "int", nullable: false),
                    PrimaryStyle = table.Column<int>(type: "int", nullable: false),
                    Perk1 = table.Column<int>(type: "int", nullable: false),
                    Perk2 = table.Column<int>(type: "int", nullable: false),
                    Perk3 = table.Column<int>(type: "int", nullable: false),
                    Perk4 = table.Column<int>(type: "int", nullable: false),
                    SecondaryStyle = table.Column<int>(type: "int", nullable: false),
                    SubPerk1 = table.Column<int>(type: "int", nullable: false),
                    SubPerk2 = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Flex = table.Column<int>(type: "int", nullable: false),
                    Offense = table.Column<int>(type: "int", nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false),
                    GamesWon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matchup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildId = table.Column<int>(type: "int", nullable: false),
                    OpponentChampionId = table.Column<int>(type: "int", nullable: false),
                    WinRate = table.Column<double>(type: "float", nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matchup_Builds_BuildId",
                        column: x => x.BuildId,
                        principalTable: "Builds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Builds_ChampionId_Position_Item0_Item1_Item2_Spell1_Spell2_PrimaryStyle_Perk1_Perk2_Perk3_Perk4_SecondaryStyle_SubPerk1_SubP~",
                table: "Builds",
                columns: new[] { "ChampionId", "Position", "Item0", "Item1", "Item2", "Spell1", "Spell2", "PrimaryStyle", "Perk1", "Perk2", "Perk3", "Perk4", "SecondaryStyle", "SubPerk1", "SubPerk2", "Defense", "Flex", "Offense" },
                unique: true,
                filter: "[Position] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cores_ChampionId_Position_Item0_Item1_Item2_Spell1_Spell2_PrimaryStyle_Perk1_Perk2_Perk3_Perk4_SecondaryStyle_SubPerk1_SubPe~",
                table: "Cores",
                columns: new[] { "ChampionId", "Position", "Item0", "Item1", "Item2", "Spell1", "Spell2", "PrimaryStyle", "Perk1", "Perk2", "Perk3", "Perk4", "SecondaryStyle", "SubPerk1", "SubPerk2", "Defense", "Flex", "Offense" },
                unique: true,
                filter: "[Position] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matchup_BuildId",
                table: "Matchup",
                column: "BuildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cores");

            migrationBuilder.DropTable(
                name: "Matchup");

            migrationBuilder.DropTable(
                name: "Builds");

            migrationBuilder.RenameColumn(
                name: "WardsKilled",
                table: "Participants",
                newName: "wardsKilled");

            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeamPosition",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
