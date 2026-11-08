using System.Collections.Concurrent;
using System.Threading.Channels;

namespace csm.Server.Pipeline
{
    public class SharedQueue
    {
        public Channel<string> PuuidsChannel { get; } = Channel.CreateUnbounded<string>();

        public Channel<string> MatchIdsChannel { get; } = Channel.CreateUnbounded<string>();

        public Channel<DTO.Match.MatchInfo> MatchDataChannel { get; } = Channel.CreateUnbounded<DTO.Match.MatchInfo>();

        public ConcurrentDictionary<string,bool> VisitedMatches { get; } = new ConcurrentDictionary<string, bool>();

        public ConcurrentDictionary<string, bool> VisitedPuuids { get; } = new ConcurrentDictionary<string, bool>();
    }
}
