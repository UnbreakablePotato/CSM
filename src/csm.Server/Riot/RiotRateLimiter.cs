using System.Net;

namespace csm.Server.Riot
{
    public class RiotRateLimiter
    {
        private readonly object _lock = new();
        private DateTime _backoffUntilUtc = DateTime.MinValue;

        public async Task WaitIfNeededAsync()
        {
            while (true)
            {
                TimeSpan delay;

                lock (_lock)
                {
                    delay = _backoffUntilUtc - DateTime.UtcNow;
                }

                if (delay <= TimeSpan.Zero)
                {
                    return;
                }

                await Task.Delay(delay);
            }
        }

        public bool HandleRateLimitHeaders(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                int retryAfterSeconds = 10;

                if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                {
                    if (int.TryParse(retryAfterValues.FirstOrDefault(), out int parsedSeconds) && parsedSeconds > 0)
                    {
                        retryAfterSeconds = parsedSeconds;
                    }
                }

                // Add a 1-second safety buffer so all workers resume cleanly after the window resets
                int waitSeconds = retryAfterSeconds + 1;
                SetBackoff(TimeSpan.FromSeconds(waitSeconds));
                Console.WriteLine($"Rate limit exceeded (429). Pausing all Riot requests for {waitSeconds} seconds...");
                return true;
            }

            CheckProactiveLimit(response, "X-App-Rate-Limit", "X-App-Rate-Limit-Count");
            CheckProactiveLimit(response, "X-Method-Rate-Limit", "X-Method-Rate-Limit-Count");

            return false;
        }

        private void CheckProactiveLimit(HttpResponseMessage response, string limitHeader, string countHeader)
        {
            if (!response.Headers.TryGetValues(limitHeader, out var limitValues) ||
                !response.Headers.TryGetValues(countHeader, out var countValues))
            {
                return;
            }

            string? limitRaw = limitValues.FirstOrDefault();
            string? countRaw = countValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(limitRaw) || string.IsNullOrWhiteSpace(countRaw))
            {
                return;
            }

            string[] limits = limitRaw.Split(',');
            string[] counts = countRaw.Split(',');

            for (int i = 0; i < Math.Min(limits.Length, counts.Length); i++)
            {
                string[] limitParts = limits[i].Split(':');
                string[] countParts = counts[i].Split(':');

                if (limitParts.Length != 2 || countParts.Length != 2)
                {
                    continue;
                }

                if (int.TryParse(limitParts[0], out int maxRequests) &&
                    int.TryParse(limitParts[1], out int windowSeconds) &&
                    int.TryParse(countParts[0], out int currentRequests))
                {
                    // If we are at or within 1 request of the cap, pace requests slightly
                    if (currentRequests >= maxRequests - 1)
                    {
                        double pauseSeconds = windowSeconds <= 1 ? 1.0 : 2.0;
                        SetBackoff(TimeSpan.FromSeconds(pauseSeconds));
                    }
                }
            }
        }

        private void SetBackoff(TimeSpan duration)
        {
            DateTime candidate = DateTime.UtcNow.Add(duration);

            lock (_lock)
            {
                if (candidate > _backoffUntilUtc)
                {
                    _backoffUntilUtc = candidate;
                }
            }
        }
    }
}
