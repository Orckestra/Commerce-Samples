using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Address information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Address
    {
        /// <summary>
        /// Gets or sets type of address (SEND, RECV, INVC, 3PTY), not Null and not empty
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [StringLength(4)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets name at address
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        [StringLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets address line 1 (not null and not empty)
        /// </summary>
        [JsonProperty(PropertyName = "addressLine1")]
        [StringLength(64)]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets address line 2
        /// </summary>
        [JsonProperty(PropertyName = "addressLine2")]
        [StringLength(64)]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets address line 3
        /// </summary>
        [JsonProperty(PropertyName = "addressLine3")]
        [StringLength(64)]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets address house number
        /// </summary>
        [JsonProperty(PropertyName = "houseNo")]
        [StringLength(16)]
        public string HouseNo { get; set; }

        /// <summary>
        /// Gets or sets city
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        [StringLength(64)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets zip code or postal code
        /// </summary>
        [JsonProperty(PropertyName = "zipCode")]
        [StringLength(16)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets state
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [StringLength(16)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets country (ISO-3)
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        [StringLength(3)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets address contact
        /// </summary>
        [JsonProperty(PropertyName = "contact")]
        [StringLength(64)]
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets telephone number
        /// </summary>
        [JsonProperty(PropertyName = "telNo")]
        [StringLength(32)]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets fax number
        /// </summary>
        [JsonProperty(PropertyName = "faxNo")]
        [StringLength(32)]
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets email address
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        [StringLength(256)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets account number
        /// </summary>
        [JsonProperty(PropertyName = "accountNumber")]
        [StringLength(32)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets customer number
        /// </summary>
        [JsonProperty(PropertyName = "customerNumber")]
        [StringLength(32)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets vat number
        /// </summary>
        [JsonProperty(PropertyName = "vatNumber")]
        [StringLength(32)]
        public string VatNumber { get; set; }

        /// <summary>
        /// Gets or sets residential flag (1=residential, 0=not residential)
        /// </summary>
        [JsonProperty(PropertyName = "residential")]
        [StringLength(32)]
        public int IsResidential { get; set; }
    }
}
