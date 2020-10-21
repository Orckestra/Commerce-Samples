using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Carrier logo api request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CarrierLogoRequest
    {
        /// <summary>
        /// Gets or sets carrier to retrieve
        /// </summary>
        [JsonProperty(PropertyName = "provider")]
        [StringLength(3)]
        public string Carrier { get; set; }

        /// <summary>
        /// Gets or sets zip code
        /// </summary>
        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets country from code
        /// </summary>
        [JsonProperty(PropertyName = "countryFrom")]
        [StringLength(3)]
        public string CountryFrom { get; set; }

        /// <summary>
        /// Gets or sets country to code
        /// </summary>
        [JsonProperty(PropertyName = "countryTo")]
        [StringLength(3)]
        public string CountryTo { get; set; }
    }
}
