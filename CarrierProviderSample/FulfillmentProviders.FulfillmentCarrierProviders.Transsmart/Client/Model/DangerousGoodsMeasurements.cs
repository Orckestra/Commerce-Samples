using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Dangerous goods measurements
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DangerousGoodsMeasurements : Measurements
    {
        /// <summary>
        /// Gets or sets net explosive mass uom
        /// </summary>
        [JsonProperty(PropertyName = "netExplosiveMassUom")]
        [StringLength(2)]
        public string NetExplosiveMassUom { get; set; }

        /// <summary>
        /// Gets or sets net explosive mass
        /// </summary>
        [JsonProperty(PropertyName = "netExplosiveMass")]
        public decimal NetExplosiveMass { get; set; }
    }
}
