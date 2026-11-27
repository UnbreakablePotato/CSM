using csm.Server.Data;
using csm.Server.DTO;
using csm.Server.Riot;
using Microsoft.EntityFrameworkCore;

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

        private readonly RiotService _riotservice;

        public DBWorker(SharedQueue sharedQueue, IServiceScopeFactory serviceScopeFactory, RiotService riotservice)
        {
            _sharedQueue = sharedQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _riotservice = riotservice;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            //var match = await _sharedQueue.MatchDataChannel.Reader.ReadAsync(stoppingToken);

            await foreach (var match in _sharedQueue.MatchDataChannel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var dbContext = scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

                    string matchId = match.matchData.Metadata.MatchId;
                    string platformServer = matchId.Contains('_')
                        ? matchId.Split('_')[0].ToLowerInvariant()
                        : match.region.ToLowerInvariant();

                    // Process match
                    for (int i = 0; i < match.matchData.Info.Participants.Count; i++)
                    {
                        var matchParticipant = match.matchData.Info.Participants[i];

                        var participant = new Models.Participant
                        {
                            MatchId = matchId,
                            Puuid = matchParticipant.Puuid,
                            SummonerName = matchParticipant.RiotIdGameName,
                            SummonerLevel = matchParticipant.SummonerLevel,
                            RiotIdGameName = matchParticipant.RiotIdGameName,
                            RiotIdTagLine = matchParticipant.RiotIdTagline,
                            ProfileIcon = matchParticipant.ProfileIcon,
                            ChampionId = matchParticipant.ChampionId,
                            ChampionName = matchParticipant.ChampionName,
                            ChampionLevel = matchParticipant.ChampionLevel,
                            ChampionExperience = matchParticipant.ChampionExperience,
                            TeamId = matchParticipant.TeamId,
                            IndividualPosition = matchParticipant.IndividualPosition,
                            Lane = matchParticipant.Lane,
                            Role = matchParticipant.Role,
                            Kills = matchParticipant.Kills,
                            Deaths = matchParticipant.Deaths,
                            Assists = matchParticipant.Assists,
                            Win = matchParticipant.Win,
                            GoldEarned = matchParticipant.GoldEarned,
                            GoldSpent = matchParticipant.GoldSpent,
                            TotalMinionsKilled = matchParticipant.TotalMinionsKilled,
                            TotalDamageDealt = matchParticipant.TotalDamageDealt,
                            TotalDamageDealtToChampions = matchParticipant.TotalDamageDealtToChampions,
                            TotalDamageTaken = matchParticipant.TotalDamageTaken,
                            DamageSelfMitigated = matchParticipant.DamageSelfMitigated,
                            TotalHeal = matchParticipant.TotalHeal,
                            VisionScore = matchParticipant.VisionScore,
                            WardsPlaced = matchParticipant.WardsPlaced,
                            WardsKilled = matchParticipant.WardsKilled,
                            Item0 = matchParticipant.Item0,
                            Item1 = matchParticipant.Item1,
                            Item2 = matchParticipant.Item2,
                            Item3 = matchParticipant.Item3,
                            Item4 = matchParticipant.Item4,
                            Item5 = matchParticipant.Item5,
                            Item6 = matchParticipant.Item6,
                            SummonerSpell1Id = matchParticipant.Summoner1Id,
                            SummonerSpell2Id = matchParticipant.Summoner2Id
                        };

                        string tier = "UNRANKED";

                        if (!string.IsNullOrEmpty(matchParticipant.Puuid))
                        {
                            var existingSummoner = await dbContext.Summoners
                                .FirstOrDefaultAsync(x => x.Puuid == matchParticipant.Puuid, stoppingToken);

                            bool needsLeagueRefresh = existingSummoner == null ||
                                existingSummoner.LastUpdated < DateTime.UtcNow.AddHours(-24);

                            if (needsLeagueRefresh)
                            {
                                LeagueEntryDTO? entry = await _riotservice.GetLEDTO(match.region, matchParticipant.Puuid);

                                if (existingSummoner == null)
                                {
                                    var summoner = new Models.Summoner
                                    {
                                        Puuid = matchParticipant.Puuid,
                                        Server = platformServer,
                                        GameName = matchParticipant.RiotIdGameName,
                                        TagLine = matchParticipant.RiotIdTagline,
                                        ProfileIcon = matchParticipant.ProfileIcon,
                                        SummonerLevel = matchParticipant.SummonerLevel,
                                        Tier = entry?.Tier ?? "UNRANKED",
                                        Rank = entry?.Rank ?? string.Empty,
                                        LeaguePoints = entry?.LeaguePoints ?? 0,
                                        Wins = entry?.Wins ?? 0,
                                        Losses = entry?.Losses ?? 0,
                                        LastUpdated = DateTime.UtcNow
                                    };

                                    dbContext.Summoners.Add(summoner);
                                    tier = summoner.Tier;
                                }
                                else
                                {
                                    existingSummoner.Server = platformServer;
                                    existingSummoner.GameName = matchParticipant.RiotIdGameName;
                                    existingSummoner.TagLine = matchParticipant.RiotIdTagline;
                                    existingSummoner.ProfileIcon = matchParticipant.ProfileIcon;
                                    existingSummoner.SummonerLevel = matchParticipant.SummonerLevel;
                                    existingSummoner.Tier = entry?.Tier ?? existingSummoner.Tier;
                                    existingSummoner.Rank = entry?.Rank ?? existingSummoner.Rank;
                                    existingSummoner.LeaguePoints = entry?.LeaguePoints ?? existingSummoner.LeaguePoints;
                                    existingSummoner.Wins = entry?.Wins ?? existingSummoner.Wins;
                                    existingSummoner.Losses = entry?.Losses ?? existingSummoner.Losses;
                                    existingSummoner.LastUpdated = DateTime.UtcNow;
                                    tier = existingSummoner.Tier;
                                }
                            }
                            else
                            {
                                existingSummoner!.Server = platformServer;
                                existingSummoner.GameName = matchParticipant.RiotIdGameName;
                                existingSummoner.TagLine = matchParticipant.RiotIdTagline;
                                existingSummoner.ProfileIcon = matchParticipant.ProfileIcon;
                                existingSummoner.SummonerLevel = matchParticipant.SummonerLevel;
                                tier = existingSummoner.Tier;
                            }
                        }

                        dbContext.Participants.Add(participant);

                        var styles = matchParticipant.Perks.Styles;
                        if (styles.Count >= 2 && styles[0].Selections.Count >= 4 && styles[1].Selections.Count >= 2)
                        {
                            var build = new Models.Build
                            {
                                ChampionId = matchParticipant.ChampionId,
                                ChampionName = matchParticipant.ChampionName,
                                ChampionIcon = "",
                                Position = matchParticipant.IndividualPosition,
                                Tier = tier,
                                Item0 = matchParticipant.Item0,
                                Item1 = matchParticipant.Item1,
                                Item2 = matchParticipant.Item2,
                                Item3 = matchParticipant.Item3,
                                Item4 = matchParticipant.Item4,
                                Item5 = matchParticipant.Item5,
                                Item6 = matchParticipant.Item6,
                                PrimaryStyle = styles[0].Style,
                                Perk1 = styles[0].Selections[0].Perk,
                                Perk2 = styles[0].Selections[1].Perk,
                                Perk3 = styles[0].Selections[2].Perk,
                                Perk4 = styles[0].Selections[3].Perk,
                                SecondaryStyle = styles[1].Style,
                                SubPerk1 = styles[1].Selections[0].Perk,
                                SubPerk2 = styles[1].Selections[1].Perk,
                                Defense = matchParticipant.Perks.StatPerks.Defense,
                                Flex = matchParticipant.Perks.StatPerks.Flex,
                                Offense = matchParticipant.Perks.StatPerks.Offense,
                                Spell1 = matchParticipant.Summoner1Id,
                                Spell2 = matchParticipant.Summoner2Id
                            };

                            dbContext.Builds.Add(build);
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while executing DBWorker service: {ex.Message}");
                }
            }
        }
    }
}
