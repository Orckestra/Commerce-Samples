using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Shipment data from shipment retrieval
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Shipment : ShipmentDetails
    {
        /// <summary>
        /// Gets or sets airway bill number
        /// </summary>
        [JsonProperty(PropertyName = "airwayBillNumber")]
        public string AirwayBillNumber { get; set; }

        /// <summary>
        /// Gets or sets tracking and trace url
        /// </summary>
        [JsonProperty(PropertyName = "trackingAndTraceUrl")]
        public string TrackingAndTraceUrl { get; set; }

        /// <summary>
        /// Gets or sets selected carrier
        /// </summary>
        [JsonProperty(PropertyName = "selectedCarrier")]
        public string SelectedCarrier { get; set; }

        /// <summary>
        /// Gets or sets executing carrier
        /// </summary>
        [JsonProperty(PropertyName = "executingCarrier")]
        public string ExecutingCarrier { get; set; }

    }
}
