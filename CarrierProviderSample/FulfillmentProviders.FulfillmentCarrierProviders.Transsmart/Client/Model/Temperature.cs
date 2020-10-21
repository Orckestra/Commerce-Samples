using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Temperature info for dangerous goods
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Temperature
    {
        /// <summary>
        /// Gets or sets flashPoint temperature (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "flashPoint")]
        public decimal FlashPoint { get; set; }

        /// <summary>
        /// Gets or sets control temperature (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "controlTemperature")]
        public decimal ControlTemperature { get; set; }

        /// <summary>
        /// Gets or sets emergency temperature (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "emergencyTemperature")]
        public decimal EmergencyTemperature { get; set; }

        /// <summary>
        /// Gets or sets temperature uom
        /// </summary>
        [JsonProperty(PropertyName = "temperatureUom")]
        [StringLength(1)]
        public string TemperatureUom { get; set; }
    }
}
