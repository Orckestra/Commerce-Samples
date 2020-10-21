using Orckestra.Overture.Caching;
using System.Threading.Tasks;
using Transsmart.Client.Model;

namespace Transsmart.Client
{
    /// <summary>
    /// Token provider for Transsmart
    /// </summary>
    public interface ITranssmartTokenProvider
    {
        /// <summary>
        /// Get the token from the cache, or generate a new one if expired or not yet cached
        /// </summary>
        /// <param name="client">proxy client</param>
        /// <returns>usable token</returns>
        Task<string> GetToken(ITranssmartClientProxy client);
    }
}