using Orckestra.Overture.Caching;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Transsmart.Client
{
    /// <summary>
    /// Token provider for Transsmart
    /// Note that tokens are good for 24 hours only, and need to be regenerated before that expiry
    /// </summary>
    public class TranssmartTokenProvider : ITranssmartTokenProvider
    {
        private TimeSpan semaphoreTimeout = new TimeSpan(0, 2, 0);
        private SemaphoreSlim semaphoreSlimToken = new SemaphoreSlim(1, 1); // Only one thread at a time can lock
        private Dictionary<string, TokenInfo> _tokenStorage = new Dictionary<string, TokenInfo>();

        /// <summary>
        /// Token Info for cache storage
        /// </summary>
        private class TokenInfo
        {
            /// <summary>
            /// Gets or sets the token used for api calls within Transsmart
            /// </summary>
            public string Token { get; set; }

            /// <summary>
            /// Gets or sets the expiry time for the token
            /// </summary>
            public DateTime Expiry { get; set; }
        }

        /// <summary>
        /// Get the current token
        /// </summary>
        /// <param name="client">proxy client</param>
        /// <returns>usable token</returns>
        public async Task<string> GetToken(ITranssmartClientProxy client)
        {
            var key = new CacheKey("Transsmart", $"Token:{client.ApiEndPoint}:{client.ApiUsername}").GetFullCacheKey(); 
            var successWait = await semaphoreSlimToken.WaitAsync(semaphoreTimeout).ConfigureAwaitWithCulture(false);
            if (!successWait) throw new TimeoutException($"Timeout waiting for cache token semaphore of {semaphoreTimeout} seconds");

            try
            {
                if (!_tokenStorage.ContainsKey(key) || DateTime.UtcNow > _tokenStorage[key].Expiry)
                {
                    var result = await client.GetToken().ConfigureAwaitWithCulture(false);
                    _tokenStorage[key] = new TokenInfo
                    {
                        Token = result.Token,
                        Expiry = DateTime.UtcNow.AddHours(23)
                    };
                }
                return _tokenStorage[key].Token;
            }
            finally
            {
                semaphoreSlimToken.Release();
            }
        }
    }
}
