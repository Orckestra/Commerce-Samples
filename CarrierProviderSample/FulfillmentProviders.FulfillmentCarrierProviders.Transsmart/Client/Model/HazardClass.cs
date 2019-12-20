using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Hazard class info for dangerous goods
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class HazardClass
    {
        /// <summary>
        /// Gets or sets primary
        /// </summary>
        [JsonProperty(PropertyName = "primary")]
        [StringLength(3)]
        public string Primary { get; set; }

        /// <summary>
        /// Gets or sets primary
        /// </summary>
        [JsonProperty(PropertyName = "secondary")]
        [StringLength(3)]
        public string Secondary { get; set; }
        /// <summary>
        /// Gets or sets tertiary
        /// </summary>
        [JsonProperty(PropertyName = "tertiary")]
        [StringLength(3)]
        public string Tertiary { get; set; }
    }
}
