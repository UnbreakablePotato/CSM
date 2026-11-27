using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSummonersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Puuid",
                table: "Participants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchId",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE [Participants] SET [MatchId] = [Id];");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Participants");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.AddColumn<string>(
                name: "ChampionName",
                table: "Builds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesPlayed",
                table: "Builds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tier",
                table: "Builds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Winrate",
                table: "Builds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Summoners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Puuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Server = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TagLine = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ProfileIcon = table.Column<int>(type: "int", nullable: false),
                    SummonerLevel = table.Column<int>(type: "int", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaguePoints = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summoners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Puuid",
                table: "Participants",
                column: "Puuid");

            migrationBuilder.CreateIndex(
                name: "IX_Summoners_Puuid",
                table: "Summoners",
                column: "Puuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summoners_Server_GameName_TagLine",
                table: "Summoners",
                columns: new[] { "Server", "GameName", "TagLine" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summoners");

            migrationBuilder.DropIndex(
                name: "IX_Participants_Puuid",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Participants");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Participants",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE [Participants] SET [Id] = LEFT([MatchId], 450);");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ChampionName",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "GamesPlayed",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "Winrate",
                table: "Builds");

            migrationBuilder.AlterColumn<string>(
                name: "Puuid",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
