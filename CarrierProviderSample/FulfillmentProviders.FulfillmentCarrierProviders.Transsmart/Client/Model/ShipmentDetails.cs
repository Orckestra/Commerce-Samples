using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Shipment details
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ShipmentDetails
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        [StringLength(32)]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets cost center
        /// </summary>
        [JsonProperty(PropertyName = "costCenter")]
        [StringLength(32)]
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets value currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets total value of the shipment (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets pickup date
        /// </summary>
        [JsonProperty(PropertyName = "pickupDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? PickupDate { get; set; }

        /// <summary>
        /// Gets or sets pickup time start (Format HH:mm)
        /// </summary>
        [JsonProperty(PropertyName = "pickupTime")]
        [StringLength(5)]
        public string PickupTimeFrom { get; set; }

        /// <summary>
        /// Gets or sets pickup time to (Format HH:mm)
        /// </summary>
        [JsonProperty(PropertyName = "pickupTimeTo")]
        [StringLength(5)]
        public string PickupTimeTo { get; set; }

        /// <summary>
        /// Gets or sets delivery date
        /// </summary>
        [JsonProperty(PropertyName = "requestedDeliveryDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? RequestedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets delivery time start (Format HH:mm)
        /// </summary>
        [JsonProperty(PropertyName = "requestedDeliveryTime")]
        [StringLength(5)]
        public string RequestedDeliveryTimeFrom { get; set; }

        /// <summary>
        /// Gets or sets delivery time to (Format HH:mm)
        /// </summary>
        [JsonProperty(PropertyName = "requestedDeliveryTimeTo")]
        [StringLength(5)]
        public string RequestedDeliveryTimeTo { get; set; }

        /// <summary>
        /// Gets or sets service (The service: example NON-DOCS/DOCS >> may indicate if a shipment goes out of the EU)
        /// </summary>
        [JsonProperty(PropertyName = "service")]
        [StringLength(16)]
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets service level time
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelTime")]
        [StringLength(32)]
        public string ServiceLevelTime { get; set; }

        /// <summary>
        /// Gets or sets service level other
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelOther")]
        [StringLength(32)]
        public string ServiceLevelOther { get; set; }

        /// <summary>
        /// Gets or sets inco terms
        /// </summary>
        [JsonProperty(PropertyName = "incoterms")]
        [StringLength(16)]
        public string Incoterms { get; set; }

        /// <summary>
        /// Gets or sets inbound flag (outbound = 0, inbound = 1)
        /// </summary>
        [JsonProperty(PropertyName = "inbound")]
        public int Inbound { get; set; }

        /// <summary>
        /// Gets or sets load meters (precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "loadmeters")]
        public decimal Loadmeters { get; set; }

        /// <summary>
        /// Gets or sets number of packages
        /// </summary>
        [JsonProperty(PropertyName = "numberOfPackages")]
        public int NumberOfPackages { get; set; }

        /// <summary>
        /// Gets or sets number of packages
        /// </summary>
        [JsonProperty(PropertyName = "packages")]
        public Package[] Packages { get; set; }

        /// <summary>
        /// Gets or sets measurements
        /// </summary>
        [JsonProperty(PropertyName = "measurements")]
        public Measurements Measurements { get; set; }

        /// <summary>
        /// Gets or sets additonal references
        /// </summary>
        [JsonProperty(PropertyName = "additionalReferences")]
        public AdditionalReference[] AdditionalReferences { get; set; }

        /// <summary>
        /// Gets or sets addresses
        /// </summary>
        [JsonProperty(PropertyName = "addresses")]
        public Address[] Addresses { get; set; }
    }
}
