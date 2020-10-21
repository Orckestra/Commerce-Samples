using Newtonsoft.Json;
using System;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Shipment status
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ShipmentStatus
    {
        /// <summary>
        /// Gets or sets status code
        /// </summary>
        [JsonProperty(PropertyName = "statusCode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets planned delivery date
        /// </summary>
        [JsonProperty(PropertyName = "plannedDeliveryDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? PlannedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets planned delivery time (HH:mm:ss)
        /// </summary>
        [JsonProperty(PropertyName = "plannedDeliveryTime")]
        public string PlannedDeliveryTime { get; set; }
    }
}
