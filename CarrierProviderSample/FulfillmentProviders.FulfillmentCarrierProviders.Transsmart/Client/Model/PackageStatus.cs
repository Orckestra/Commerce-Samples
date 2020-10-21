using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Package status
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PackageStatus
    {
        /// <summary>
        /// Gets or sets package line sequence
        /// </summary>
        [JsonProperty(PropertyName = "sequence")]
        public string Sequence { get; set; }

        /// <summary>
        /// Gets or sets package line number
        /// </summary>
        [JsonProperty(PropertyName = "lineNo")]
        public int LineNo { get; set; }

        /// <summary>
        /// Gets or sets package line id
        /// </summary>
        [JsonProperty(PropertyName = "shipmentLineId")]
        public string ShipmentLineId { get; set; }

        /// <summary>
        /// Gets or sets airway bill number
        /// </summary>
        [JsonProperty(PropertyName = "awb")]
        public string AirwayBillNumber { get; set; }

        /// <summary>
        /// Gets or sets generic status code
        /// </summary>
        [JsonProperty(PropertyName = "genericStatusCode")]
        public string GenericStatusCode { get; set; }

        /// <summary>
        /// Gets or sets carrier status code
        /// </summary>
        [JsonProperty(PropertyName = "carrierStatusCode")]
        public string CarrierStatusCode { get; set; }

        /// <summary>
        /// Gets or sets carrier status description
        /// </summary>
        [JsonProperty(PropertyName = "carrierStatusDescription")]
        public string CarrierStatusDescription { get; set; }
    }
}
