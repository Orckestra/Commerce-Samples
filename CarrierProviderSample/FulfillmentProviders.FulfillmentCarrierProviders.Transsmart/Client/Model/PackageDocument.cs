using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// A package document
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PackageDocument
    {
        /// <summary>
        /// Gets or sets document type (e.g. Label)
        /// </summary>
        [JsonProperty(PropertyName = "docType")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets file format (e.g. PDF or ZPL)
        /// </summary>
        [JsonProperty(PropertyName = "fileFormat")]
        public string FileFormat { get; set; }

        /// <summary>
        /// Gets or sets encoding format (e.g. base64)
        /// </summary>
        [JsonProperty(PropertyName = "encodingFormat")]
        public string EncodingFormat { get; set; }

        /// <summary>
        /// Gets or sets document data
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string DocumentData { get; set; }

        /// <summary>
        /// Gets or sets printer type
        /// </summary>
        [JsonProperty(PropertyName = "printerType")]
        public string PrinterType { get; set; }

        /// <summary>
        /// Gets or sets printer name
        /// </summary>
        [JsonProperty(PropertyName = "printerName")]
        public string PrinterName { get; set; }

        /// <summary>
        /// Gets or sets document sequence
        /// </summary>
        [JsonProperty(PropertyName = "sequence")]
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets document height
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets document width
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets airway bill number
        /// </summary>
        [JsonProperty(PropertyName = "airwayBillNumber")]
        public string AirwayBillNumber { get; set; }

        /// <summary>
        /// Gets or sets line number
        /// </summary>
        [JsonProperty(PropertyName = "lineNo")]
        public int LineNo { get; set; }

        /// <summary>
        /// Gets or sets shipment line id
        /// </summary>
        [JsonProperty(PropertyName = "shipmentLineId")]
        public string ShipmentLineId { get; set; }
    }
}
