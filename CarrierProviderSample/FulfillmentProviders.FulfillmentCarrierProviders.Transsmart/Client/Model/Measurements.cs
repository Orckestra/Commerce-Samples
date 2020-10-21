using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Measurements
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Measurements
    {
        /// <summary>
        /// Gets or sets length of the object (package, dangerous good). Is ignored when used on shipment level (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public decimal Length { get; set; }

        /// <summary>
        /// Gets or sets width of the object (package, dangerous good). Is ignored when used on shipment level (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets height of the object (package, dangerous good). Is ignored when used on shipment level (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public decimal Height { get; set; }

        /// <summary>
        /// Gets or sets weight of the object (package, dangerous good). Is ignored when used on shipment level (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets calculated weight of the object (package, dangerous good). Is ignored when used on shipment level (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "calculatedWeight")]
        public decimal CalculatedWeight { get; set; }

        /// <summary>
        /// Gets or sets linear unit of measure (e.g. CM, FT, IN, YD)
        /// </summary>
        [JsonProperty(PropertyName = "linearUom")]
        [StringLength(3)]
        public string LinearUom { get; set; }

        /// <summary>
        /// Gets or sets mass unit of measure (e.g. KG, LB, OZ)
        /// </summary>
        [JsonProperty(PropertyName = "massUom")]
        [StringLength(3)]
        public string MassUom { get; set; }
    }
}
