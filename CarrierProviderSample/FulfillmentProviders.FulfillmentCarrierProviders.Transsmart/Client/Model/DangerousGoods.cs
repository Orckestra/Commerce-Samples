using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Dangerous goods line information, not implemented
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DangerousGoods
    {
        /// <summary>
        /// Gets or sets id code
        /// </summary>
        [JsonProperty(PropertyName = "idCode")]
        [StringLength(64)]
        public string IdCode { get; set; }

        /// <summary>
        /// Gets or sets Un code
        /// </summary>
        [JsonProperty(PropertyName = "unCode")]
        [StringLength(64)]
        public string UnCode { get; set; }

        /// <summary>
        /// Gets or sets Un subcode
        /// </summary>
        [JsonProperty(PropertyName = "unSubCode")]
        [StringLength(64)]
        public string UnSubCode { get; set; }

        /// <summary>
        /// Gets or sets packing group
        /// </summary>
        [JsonProperty(PropertyName = "packingGroup")]
        [StringLength(64)]
        public string PackingGroup { get; set; }

        /// <summary>
        /// Gets or sets packing type
        /// </summary>
        [JsonProperty(PropertyName = "packingType")]
        [StringLength(64)]
        public string PackingType { get; set; }

        /// <summary>
        /// Gets or sets packing classification
        /// </summary>
        [JsonProperty(PropertyName = "packingClassification")]
        [StringLength(64)]
        public string PackingClassification { get; set; }

        /// <summary>
        /// Gets or sets quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets limited quantity
        /// </summary>
        [JsonProperty(PropertyName = "limitedQuantity")]
        public int LimitedQuantity { get; set; }

        /// <summary>
        /// Gets or sets limited quantity point
        /// </summary>
        [JsonProperty(PropertyName = "limitedQuantityPoints")]
        public int LimitedQuantityPoints { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        [StringLength(128)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets marking identifier
        /// </summary>
        [JsonProperty(PropertyName = "markingIdentifier")]
        [StringLength(64)]
        public string MarkingIdentifier { get; set; }

        /// <summary>
        /// Gets or sets instruction
        /// </summary>
        [JsonProperty(PropertyName = "instruction")]
        [StringLength(128)]
        public string Instruction { get; set; }

        /// <summary>
        /// Gets or sets flash point degree (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "flashPointDegree")]
        public decimal FlashPointDegree { get; set; }

        /// <summary>
        /// Gets or sets tunnel code
        /// </summary>
        [JsonProperty(PropertyName = "tunnelCode")]
        [StringLength(16)]
        public string TunnelCode { get; set; }

        /// <summary>
        /// Gets or sets dangerous goods measurements
        /// </summary>
        [JsonProperty(PropertyName = "measurements")]
        public DangerousGoodsMeasurements Measurements { get; set; }

        /// <summary>
        /// Gets or sets hazardous substance indicator
        /// </summary>
        [JsonProperty(PropertyName = "isHazardousSubstance")]
        public bool IsHazardousSubstance { get; set; }

        /// <summary>
        /// Gets or sets regulation information
        /// </summary>
        [JsonProperty(PropertyName = "regulation")]
        public Regulation Regulation { get; set; }

        /// <summary>
        /// Gets or sets hazard class information
        /// </summary>
        [JsonProperty(PropertyName = "hazardClass")]
        public HazardClass HazardClass { get; set; }

        /// <summary>
        /// Gets or sets net weight (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "netWeight")]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// Gets or sets volume (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public decimal Volume { get; set; }
    }
}
