using Newtonsoft.Json;
using OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart;
using ServiceStack.DataAnnotations;
using System;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Book and Print api request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BookRequest
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        [StringLength(32)]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets carrier code (e.g. UPS)
        /// </summary>
        [JsonProperty(PropertyName = "carrier")]
        [StringLength(3)]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets cost center
        /// </summary>
        [JsonProperty(PropertyName = "costCenter")]
        [StringLength(32)]
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets mail type
        /// </summary>
        [JsonProperty(PropertyName = "mailType")]
        public int MailType { get; set; }

        /// <summary>
        /// Gets or sets language code (ISO-2)
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        [StringLength(2)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        [StringLength(128)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets instruction
        /// </summary>
        [JsonProperty(PropertyName = "instruction")]
        [StringLength(128)]
        public string Instruction { get; set; }

        /// <summary>
        /// Gets or sets value currency
        /// </summary>
        [JsonProperty(PropertyName = "valueCurrency")]
        [StringLength(3)]
        public string ValueCurrency { get; set; }

        /// <summary>
        /// Gets or sets total value of the shipment (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets total spot price of the shipment (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "spotPrice")]
        public decimal SpotPrice { get; set; }

        /// <summary>
        /// Gets or sets spot price currency
        /// </summary>
        [JsonProperty(PropertyName = "spotPriceCurrency")]
        [StringLength(3)]
        public string SpotPriceCurrency { get; set; }

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
        /// Gets or sets delivery note information
        /// </summary>
        [JsonProperty(PropertyName = "deliveryNoteInformation")]
        public DeliveryNoteInformation DeliveryNoteInformation { get; set; }

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
