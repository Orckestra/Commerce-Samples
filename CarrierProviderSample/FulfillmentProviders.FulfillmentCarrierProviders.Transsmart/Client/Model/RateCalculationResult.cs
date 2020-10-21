using Newtonsoft.Json;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a rate calulcation api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RateCalculationResult
    {
        /// <summary>
        /// Gets or sets shipment details
        /// </summary>
        [JsonProperty(PropertyName = "shipmentDetails")]
        public ShipmentDetails ShipmentDetails { get; set; }

        /// <summary>
        /// Gets or sets calculated rates
        /// </summary>
        [JsonProperty(PropertyName = "rates")]
        public Rate[] Rates { get; set; }

        /// <summary>
        /// Gets or sets errors during calculation
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public TranssmartError Errors { get; set; }
    }
}
