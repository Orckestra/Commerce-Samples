using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a carrier logo request api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CarrierLogoResult
    {
        /// <summary>
        /// Gets or sets logo sources
        /// </summary>
        [JsonProperty(PropertyName = "sources")]
        public CarrierLogoSource[] Sources { get; set; }
    }
}
