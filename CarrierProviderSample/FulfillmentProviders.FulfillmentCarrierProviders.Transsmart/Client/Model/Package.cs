using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Package information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Package
    {
        /// <summary>
        /// Gets or sets line number
        /// </summary>
        [JsonProperty(PropertyName = "lineNo")]
        public int LineNo { get; set; }

        /// <summary>
        /// Gets or sets shipment line id
        /// </summary>
        [JsonProperty(PropertyName = "shipmentLineId")]
        [StringLength(32)]
        public string ShipmentLineId { get; set; }

        /// <summary>
        /// Gets or sets package type
        /// </summary>
        [JsonProperty(PropertyName = "packageType")]
        [StringLength(16)]
        public string PackageType { get; set; }

        /// <summary>
        /// Gets or sets description of the goods
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        [StringLength(128)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets whether the package is stackable
        /// </summary>
        [JsonProperty(PropertyName = "stackable")]
        public bool IsStackable { get; set; }

        /// <summary>
        /// Gets or sets stack height
        /// </summary>
        [JsonProperty(PropertyName = "stackHeight")]
        public int StackHeight { get; set; }

        /// <summary>
        /// Gets or sets additonal references
        /// </summary>
        [JsonProperty(PropertyName = "additionalReferences")]
        public AdditionalReference[] AdditionalReferences { get; set; }

        /// <summary>
        /// Gets or sets delivery note information
        /// </summary>
        [JsonProperty(PropertyName = "deliveryNoteInfo")]
        public DeliveryNoteInformation DeliveryNoteInfo { get; set; }

        /// <summary>
        /// Gets or sets dangerous goods information
        /// </summary>
        [JsonProperty(PropertyName = "dangerousGoodsInformation")]
        public DeliveryNoteInformation DangerousGoodsInformation { get; set; }

        /// <summary>
        /// Gets or sets measurements
        /// </summary>
        [JsonProperty(PropertyName = "measurements")]
        public Measurements Measurements { get; set; }
    }
}
