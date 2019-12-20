using Newtonsoft.Json;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a print api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PrintResult
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets airway bill number
        /// </summary>
        [JsonProperty(PropertyName = "airwayBillNumber")]
        public string AirwayBillNumber { get; set; }

        /// <summary>
        /// Gets or sets shipment documents
        /// </summary>
        [JsonProperty(PropertyName = "shipmentDocs")]
        public ShipmentDoc[] ShipmentDocs { get; set; }
        /// <summary>
        /// Gets or sets package documents
        /// </summary>
        [JsonProperty(PropertyName = "packageDocs")]
        public PackageDocument[] PackageDocs { get; set; }

        /// <summary>
        /// Gets or sets errors
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public TranssmartError[] Errors { get; set; }
    }
}
