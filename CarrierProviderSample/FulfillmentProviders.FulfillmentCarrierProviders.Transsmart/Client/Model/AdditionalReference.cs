using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Additional references
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AdditionalReference
    {
        /// <summary>
        /// Gets or sets type of the additional reference
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [StringLength(64)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets additional reference value
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        [StringLength(1024)]
        public string Value { get; set; }
    }
}
