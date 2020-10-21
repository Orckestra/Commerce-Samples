using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transsmart.Client
{
    /// <summary>
    /// Contains all the Transsmart information needed for a quote
    /// </summary>
    public class TranssmartQuoteInfo
    {
        /// <summary>
        /// Gets or sets reference for the information
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets language code (ISO-2)
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the carrier name
        /// </summary>
        public string Carrier { get; set; } // ex: UPS, FEDEX, USPS

        /// <summary>
        /// Gets or sets the carrier service
        /// </summary>
        public string CarrierService { get; set; }

        /// <summary>
        /// Gets or sets the carrier service time
        /// </summary>
        public string CarrierServiceTime { get; set; }

        /// <summary>
        /// Gets or sets the carrier service other data
        /// </summary>
        public string CarrierServiceOther { get; set; }
    }
}
