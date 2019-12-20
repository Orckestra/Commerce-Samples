using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Delivery note line
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DeliveryNoteLine
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
        /// Gets or sets country origin
        /// </summary>
        [JsonProperty(PropertyName = "countryOrigin")]
        [StringLength(3)]
        public string CountryOrigin { get; set; }

        /// <summary>
        /// Gets or sets line number
        /// </summary>
        [JsonProperty(PropertyName = "lineNumber")]
        public int LineNumber { get; set; }

        /// <summary>
        /// Gets or sets quantity
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets quantity uom (eg. PCS,BOX)
        /// </summary>
        [JsonProperty(PropertyName = "quantityUom")]
        [StringLength(32)]
        public string QuantityUom { get; set; }

        /// <summary>
        /// Gets or sets quantity that is ordered
        /// </summary>
        [JsonProperty(PropertyName = "quantityOrder")]
        public int QuantityOrder { get; set; }

        /// <summary>
        /// Gets or sets quantity that is back ordered
        /// </summary>
        [JsonProperty(PropertyName = "quantityBackorder")]
        public int QuantityBackorder { get; set; }

        /// <summary>
        /// Gets or sets article id
        /// </summary>
        [JsonProperty(PropertyName = "articleId")]
        [StringLength(64)]
        public string ArticleId { get; set; }

        /// <summary>
        /// Gets or sets article name
        /// </summary>
        [JsonProperty(PropertyName = "articleName")]
        [StringLength(128)]
        public string ArticleName { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets HS code (Harmonized System)
        /// </summary>
        [JsonProperty(PropertyName = "hsCode")]
        [StringLength(25)]
        public string HSCode { get; set; }

        /// <summary>
        /// Gets or sets HS code description (Harmonized System)
        /// </summary>
        [JsonProperty(PropertyName = "hsCodeDescription")]
        [StringLength(128)]
        public string HSCodeDescription { get; set; }

        /// <summary>
        /// Gets or sets price per article (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets serial number
        /// </summary>
        [JsonProperty(PropertyName = "serialNumber")]
        [StringLength(64)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets reason of export
        /// </summary>
        [JsonProperty(PropertyName = "reasonOfExport")]
        [StringLength(64)]
        public string ReasonOfExport { get; set; }

        /// <summary>
        /// Gets or sets date for on the proforma invoice (Format yyyyMMdd)
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceDate")]
        [StringLength(8)]
        public string ProformaInvoiceDate { get; set; }

        /// <summary>
        /// Gets or sets proforma invoice number
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceNumber")]
        [StringLength(15)]
        public string ProformaInvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets proforma invoice line number
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceLineNumber")]
        [StringLength(15)]
        public string ProformaInvoiceLineNumber { get; set; }

        /// <summary>
        /// Gets or sets quantity m2 (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "quantityM2")]
        public decimal QuantityM2 { get; set; }

        /// <summary>
        /// Gets or sets customer order
        /// </summary>
        [JsonProperty(PropertyName = "customerOrder")]
        [StringLength(64)]
        public string CustomerOrder { get; set; }

        /// <summary>
        /// Gets or sets article ean code
        /// </summary>
        [JsonProperty(PropertyName = "articleEanCode")]
        [StringLength(64)]
        public string ArticleEanCode { get; set; }

        /// <summary>
        /// Gets or sets quality
        /// </summary>
        [JsonProperty(PropertyName = "quality")]
        [StringLength(64)]
        public string Quality { get; set; }

        /// <summary>
        /// Gets or sets composition
        /// </summary>
        [JsonProperty(PropertyName = "composition")]
        [StringLength(64)]
        public string Composition { get; set; }

        /// <summary>
        /// Gets or sets assembly instructions
        /// </summary>
        [JsonProperty(PropertyName = "assemblyInstructions")]
        [StringLength(65535)]
        public string AssemblyInstructions { get; set; }

        /// <summary>
        /// Gets or sets gross weight (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "grossWeight")]
        public decimal GrossWeight { get; set; }

        /// <summary>
        /// Gets or sets nett weight (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "nettWeight")]
        public decimal NettWeight { get; set; }

        /// <summary>
        /// Gets or sets nett price (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "nettPrice")]
        public decimal NettPrice { get; set; }

        /// <summary>
        /// Gets or sets weight uom (eg. KG, LB, OZ)
        /// </summary>
        [JsonProperty(PropertyName = "weightUom")]
        [StringLength(3)]
        public string WeightUom { get; set; }

        /// <summary>
        /// Gets or sets freight charges for on the proforma invoice (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceFreightCharges")]
        public decimal ProformaInvoiceFreightCharges { get; set; }

        /// <summary>
        /// Gets or sets insurance charges for on the proforma invoice (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceInsuranceCharges")]
        public decimal ProformaInvoiceInsuranceCharges { get; set; }

        /// <summary>
        /// Gets or sets discount on the proforma invoice (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceDiscounts")]
        public decimal ProformaInvoiceDiscounts { get; set; }

        /// <summary>
        /// Gets or sets other charges on the proforma invoice (Precision of 2)
        /// </summary>
        [JsonProperty(PropertyName = "proformaInvoiceOtherCharges")]
        public decimal ProformaInvoiceOtherCharges { get; set; }
    }
}
