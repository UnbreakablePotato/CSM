using System.Collections.Frozen;

namespace csm.Server.Riot
{

    public sealed record RouteInfo(string Platform, string Regional);
    public static class RegionRouter
    {

        private static readonly FrozenDictionary<string, RouteInfo> Routes = new Dictionary<string, RouteInfo>(StringComparer.OrdinalIgnoreCase)
        {
            ["euw"] = new("euw1", "europe"),
            ["na"] = new("na1", "americas"),
            ["eune"] = new("eun1", "europe"),
            ["kr"] = new("kr", "asia"),
        }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

        public static RouteInfo Resolve(string alias)
        {
            RouteInfo info;

            if (Routes.TryGetValue(alias, out info))
            {
                return info;
            }
            else
            {
                throw new Exception($"Unknown region alias: {alias}");
            }
        }

    }
}
