using csm.Server.Data;

namespace csm.Server.Pipeline
{
    /*
     DBWorker should add build, core, and the remaining match data to the database.
     
     Might need to be a background service?
          
     */
    public class DBWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DBWorker(SharedQueue sharedQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _sharedQueue = sharedQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var match = await _sharedQueue.MatchDataChannel.Reader.ReadAsync(stoppingToken);

            for (int i = 0; i < match.Info.Participants.Count; i++)
            {
                var participant = new Models.Participant
                {
                    Id = match.Metadata.MatchId,
                    Puuid = match.Info.Participants[i].Puuid,
                    SummonerName = match.Info.Participants[i].RiotIdGameName,
                    SummonerLevel = match.Info.Participants[i].SummonerLevel,
                    RiotIdGameName = match.Info.Participants[i].RiotIdGameName,
                    RiotIdTagLine = match.Info.Participants[i].RiotIdTagline,
                    ProfileIcon = match.Info.Participants[i].ProfileIcon,
                    ChampionId = match.Info.Participants[i].ChampionId,
                    ChampionName = match.Info.Participants[i].ChampionName,
                    ChampionLevel = match.Info.Participants[i].ChampionLevel,
                    ChampionExperience = match.Info.Participants[i].ChampionExperience,
                    TeamId = match.Info.Participants[i].TeamId,
                    IndividualPosition = match.Info.Participants[i].IndividualPosition,
                    Lane = match.Info.Participants[i].Lane,
                    Role = match.Info.Participants[i].Role,
                    Kills = match.Info.Participants[i].Kills,
                    Deaths = match.Info.Participants[i].Deaths,
                    Assists = match.Info.Participants[i].Assists,
                    Win = match.Info.Participants[i].Win,
                    GoldEarned = match.Info.Participants[i].GoldEarned,
                    GoldSpent = match.Info.Participants[i].GoldSpent,
                    TotalMinionsKilled = match.Info.Participants[i].TotalMinionsKilled,
                    TotalDamageDealt = match.Info.Participants[i].TotalDamageDealt,
                    TotalDamageDealtToChampions = match.Info.Participants[i].TotalDamageDealtToChampions,
                    TotalDamageTaken = match.Info.Participants[i].TotalDamageTaken,
                    DamageSelfMitigated = match.Info.Participants[i].DamageSelfMitigated,
                    TotalHeal = match.Info.Participants[i].TotalHeal,
                    VisionScore = match.Info.Participants[i].VisionScore,
                    WardsPlaced = match.Info.Participants[i].WardsPlaced,
                    WardsKilled = match.Info.Participants[i].WardsKilled,
                    Item0 = match.Info.Participants[i].Item0,
                    Item1 = match.Info.Participants[i].Item1,
                    Item2 = match.Info.Participants[i].Item2,
                    Item3 = match.Info.Participants[i].Item3,
                    Item4 = match.Info.Participants[i].Item4,
                    Item5 = match.Info.Participants[i].Item5,
                    Item6 = match.Info.Participants[i].Item6

                };
                dbContext.Participants.Add(participant);

                await dbContext.SaveChangesAsync(stoppingToken);

                var build = new Models.Build
                {
                    ChampionId = match.Info.Participants[i].ChampionId,
                    ChampionIcon = "",
                    Position = match.Info.Participants[i].IndividualPosition,
                    Item0 = match.Info.Participants[i].Item0,
                    Item1 = match.Info.Participants[i].Item1,
                    Item2 = match.Info.Participants[i].Item2,
                    Item3 = match.Info.Participants[i].Item3,
                    Item4 = match.Info.Participants[i].Item4,
                    Item5 = match.Info.Participants[i].Item5,
                    Item6 = match.Info.Participants[i].Item6,
                    PrimaryStyle = match.Info.Participants[i].Perks.Styles[0].Style,
                    Perk1 = match.Info.Participants[i].Perks.Styles[0].Selections[0].Perk,
                    Perk2 = match.Info.Participants[i].Perks.Styles[0].Selections[1].Perk,
                    Perk3 = match.Info.Participants[i].Perks.Styles[0].Selections[2].Perk,
                    Perk4 = match.Info.Participants[i].Perks.Styles[0].Selections[3].Perk,
                    SecondaryStyle = match.Info.Participants[i].Perks.Styles[1].Style,
                    SubPerk1 = match.Info.Participants[i].Perks.Styles[1].Selections[0].Perk,
                    SubPerk2 = match.Info.Participants[i].Perks.Styles[1].Selections[1].Perk,
                    Defense = match.Info.Participants[i].Perks.StatPerks.Defense,
                    Flex = match.Info.Participants[i].Perks.StatPerks.Flex,
                    Offense = match.Info.Participants[i].Perks.StatPerks.Offense,
                    Spell1 = match.Info.Participants[i].Summoner1Id,
                    Spell2 = match.Info.Participants[i].Summoner2Id

                };
               // dbContext.Build.Add(build);
                //await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
