using Newtonsoft.Json;
using OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart;
using ServiceStack.DataAnnotations;
using System;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a book and print api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BookResult
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets airway bill number
        /// </summary>
        [JsonProperty(PropertyName = "awb")]
        public string AirwayBillNumber { get; set; }

        /// <summary>
        /// Gets or sets carrier code (e.g. UPS)
        /// </summary>
        [JsonProperty(PropertyName = "carrier")]
        [StringLength(3)]
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets executing carrier code (e.g. UPS)
        /// </summary>
        [JsonProperty(PropertyName = "executingCarrier")]
        [StringLength(3)]
        public string ExecutingCarrierCode { get; set; }

        /// <summary>
        /// Gets or sets pieces (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "pieces")]
        public decimal Pieces { get; set; }

        /// <summary>
        /// Gets or sets weight (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public decimal Weight { get; set; }

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
        /// Gets or sets inco terms
        /// </summary>
        [JsonProperty(PropertyName = "incoterms")]
        [StringLength(16)]
        public string Incoterms { get; set; }

        /// <summary>
        /// Gets or sets price (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets value currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets pickup date
        /// </summary>
        [JsonProperty(PropertyName = "pickupDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? PickupDate { get; set; }

        /// <summary>
        /// Gets or sets tracking url
        /// </summary>
        [JsonProperty(PropertyName = "trackingUrl")]
        public string TrackingUrl { get; set; }

        /// <summary>
        /// Gets or sets shipment status
        /// </summary>
        [JsonProperty(PropertyName = "shipmentStatus")]
        public ShipmentStatus ShipmentStatus { get; set; }

        /// <summary>
        /// Gets or sets package status
        /// </summary>
        [JsonProperty(PropertyName = "packages")]
        public PackageStatus[] PackageStatus { get; set; }
    }
}
