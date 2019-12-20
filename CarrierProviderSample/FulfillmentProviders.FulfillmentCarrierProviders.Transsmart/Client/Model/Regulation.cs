using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Regulation info for dangerous goods
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Regulation
    {
        /// <summary>
        /// Gets or sets regulation level
        /// </summary>
        [JsonProperty(PropertyName = "level")]
        [StringLength(2)]
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets regulation type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [StringLength(5)]
        public string Type { get; set; }
    }
}
