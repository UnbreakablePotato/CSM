using System.Collections.Concurrent;
using System.Threading.Channels;

namespace csm.Server.Pipeline
{
    public record Player(string region, string puuid);

    public record Match(string region, string matchid);

    public record MatchData(string region, DTO.Match.MatchResponse matchData);
    public class SharedQueue
    {
        public Channel<string> PuuidsChannel { get; } = Channel.CreateUnbounded<string>();

        public Channel<Match> MatchIdsChannel { get; } = Channel.CreateUnbounded<Match>();

        public Channel<Player> PlayerChannel { get; } = Channel.CreateUnbounded<Player>();

        public Channel<MatchData> MatchDataChannel { get; } = Channel.CreateUnbounded<MatchData>();

        public ConcurrentDictionary<string,bool> VisitedMatches { get; } = new ConcurrentDictionary<string, bool>();

        public ConcurrentDictionary<string, bool> VisitedPuuids { get; } = new ConcurrentDictionary<string, bool>();

        public long CycleStartTimeUnixSeconds { get; private set; } = DateTimeOffset.UtcNow.AddHours(-24).ToUnixTimeSeconds();

        public void ResetForNewCycle()
        {
            VisitedPuuids.Clear();
            VisitedMatches.Clear();
            CycleStartTimeUnixSeconds = DateTimeOffset.UtcNow.AddHours(-24).ToUnixTimeSeconds();
        }
    }
}
