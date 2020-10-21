using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Transsmart values for APIs
    /// </summary>
    public static class TranssmartOptions
    {
        /// <summary>
        /// Get the non document service type code
        /// </summary>
        public readonly static string ServiceTypesNonDocs = "NON-DOCS";

        /// <summary>
        /// Get the document service type code
        /// </summary>
        public readonly static string ServiceTypesDocs = "DOCS";

        /// <summary>
        /// Get the envelope service type
        /// </summary>
        public readonly static string ServiceTypesEnvelopes = "ENVELOPES";

        /// <summary>
        /// Get the valid linear uom codes
        /// </summary>
        public readonly static string[] ValidLinearUOM = new string[] { "CM", "FT", "IN", "YD" };

        /// <summary>
        /// Get the valid mass uom codes
        /// </summary>
        public readonly static string[] ValidMassUOM = new string[] { "KG", "LB", "OZ" };

        /// <summary>
        /// Get the valid service types available
        /// </summary>
        public readonly static string[] ValidServiceTypes = new string[] { ServiceTypesNonDocs, ServiceTypesDocs, ServiceTypesEnvelopes };

        /// <summary>
        /// Document type used for storage
        /// </summary>
        public enum DocType { Label };

        /// <summary>
        /// Document types
        /// </summary>
        public enum DocumentTypes { PDF, ZPL };

        /// <summary>
        /// Shipment status
        /// </summary>
        public enum ShipmentStatus { BOOK };
    }
}
