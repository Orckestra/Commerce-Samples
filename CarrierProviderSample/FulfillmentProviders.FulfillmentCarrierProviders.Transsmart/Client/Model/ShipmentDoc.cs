using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// A shipment document
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ShipmentDoc
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        [StringLength(32)]
        public string Reference { get; set; }

    }
}
