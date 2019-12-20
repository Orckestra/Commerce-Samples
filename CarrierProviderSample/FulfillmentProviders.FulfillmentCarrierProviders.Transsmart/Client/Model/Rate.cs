using Newtonsoft.Json;
using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart;
using ServiceStack.DataAnnotations;
using System;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Rate info for rate calculation
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Rate
    {
        /// <summary>
        /// Gets or sets description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        [StringLength(128)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets sales price (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "salesPrice")]
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// Gets or sets value sales currency
        /// </summary>
        [JsonProperty(PropertyName = "salesCurrency")]
        [StringLength(3)]
        public string SalesCurrency { get; set; }

        /// <summary>
        /// Gets or sets weight of the object (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets weight unit of measure (e.g. KG, LB, OZ)
        /// </summary>
        [JsonProperty(PropertyName = "weightUom")]
        [StringLength(3)]
        public string WeightUom { get; set; }

        /// <summary>
        /// Gets or sets calculated weight of the object (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "calculatedWeight")]
        public decimal CalculatedWeight { get; set; }

        /// <summary>
        /// Gets or sets calculated weight type
        /// </summary>
        [JsonProperty(PropertyName = "calculatedWeightType")]
        [StringLength(45)]
        public string CalculatedWeightType { get; set; }

        /// <summary>
        /// Gets or sets calculated weight unit of measure (e.g. KG, LB, OZ)
        /// </summary>
        [JsonProperty(PropertyName = "calculatedWeightUom")]
        [StringLength(3)]
        public string CalculatedWeightUom { get; set; }

        /// <summary>
        /// Gets or sets executing carrier
        /// </summary>
        [JsonProperty(PropertyName = "executingCarrier")]
        [StringLength(3)]
        public string ExecutingCarrier { get; set; }

        /// <summary>
        /// Gets or sets carrier
        /// </summary>
        [JsonProperty(PropertyName = "carrier")]
        [StringLength(3)]
        public string Carrier { get; set; }

        /// <summary>
        /// Gets or sets the carrier image
        /// </summary>
        public FulfillmentCarrierImage CarrierImage { get; set; }

        /// <summary>
        /// Gets or sets transit time description
        /// </summary>
        [JsonProperty(PropertyName = "transitTimeDescription")]
        [StringLength(128)]
        public string TransitTimeDescription { get; set; }

        /// <summary>
        /// Gets or sets hours of transit based on requested pickup and delivery date times
        /// </summary>
        [JsonProperty(PropertyName = "transitTimeHours")]
        public decimal TransitTimeHours { get; set; }

        /// <summary>
        /// Gets or sets carrier description
        /// </summary>
        [JsonProperty(PropertyName = "carrierDescription")]
        [StringLength(128)]
        public string CarrierDescription { get; set; }

        /// <summary>
        /// Gets or sets service level time
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelTime")]
        [StringLength(32)]
        public string ServiceLevelTime { get; set; }

        /// <summary>
        /// Gets or sets service level time description
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelTimeDescription")]
        [StringLength(128)]
        public string ServiceLevelTimeDescription { get; set; }

        /// <summary>
        /// Gets or sets service level other
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelOther")]
        [StringLength(32)]
        public string ServiceLevelOther { get; set; }

        /// <summary>
        /// Gets or sets service level other description
        /// </summary>
        [JsonProperty(PropertyName = "serviceLevelOtherDescription")]
        [StringLength(128)]
        public string ServiceLevelOtherDescription { get; set; }

        /// <summary>
        /// Gets or sets pickup date, note that this can differ from the originally desired pickup date due to the carrier’s logistics (Format yyyy-MM-dd)
        /// </summary>
        [JsonProperty(PropertyName = "pickupDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? PickupDate { get; set; }

        /// <summary>
        /// Gets or sets delivery date, note that this can differ from the originally desired delivery date due to carrier’s logistics (Format yyyy-MM-dd)
        /// </summary>
        [JsonProperty(PropertyName = "deliveryDate")]
        [JsonConverter(typeof(TranssmartDateOnlyConverter))]
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets delivery time (Format HH:mm)
        /// </summary>
        [JsonProperty(PropertyName = "deliveryTime")]
        [StringLength(5)]
        public string DeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets calculated price (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
    }
}
