using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Delivery note information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DeliveryNoteInformation
    {
        /// <summary>
        /// Gets or sets delivery note id
        /// </summary>
        [JsonProperty(PropertyName = "deliveryNoteId")]
        [StringLength(32)]
        public string DeliveryNoteId { get; set; }

        /// <summary>
        /// Gets or sets currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets price (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets delivery note lines
        /// </summary>
        [JsonProperty(PropertyName = "deliveryNoteLines")]
        public DeliveryNoteLine[] DeliveryNoteLines { get; set; }
    }
}
