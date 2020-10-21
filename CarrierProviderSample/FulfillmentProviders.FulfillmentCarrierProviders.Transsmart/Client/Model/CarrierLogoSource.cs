using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Carrier logo source
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CarrierLogoSource
    {
        /// <summary>
        /// Gets or sets carrier code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets carrier name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string CarrierName { get; set; }

        /// <summary>
        /// Gets or sets logo image in base64
        /// </summary>
        [JsonProperty(PropertyName = "logo")]
        public string CarrierLogo { get; set; }

        /// <summary>
        /// Gets or sets carrier marker image in base64
        /// </summary>
        [JsonProperty(PropertyName = "marker")]
        public string CarrierMarker { get; set; }

        /// <summary>
        /// Gets or sets errors
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public TranssmartError Errors { get; set; }
    }
}
