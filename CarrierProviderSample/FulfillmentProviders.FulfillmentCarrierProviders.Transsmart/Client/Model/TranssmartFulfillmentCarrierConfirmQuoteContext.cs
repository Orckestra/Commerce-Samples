using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using System;
using System.Collections.Generic;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Transsmart fulfillment carrier quote context
    /// </summary>
    public class TranssmartFulfillmentCarrierConfirmQuoteContext
    {
        /// <summary>
        /// Gets or sets the mappers for dealing with conversion to properties
        /// </summary>
        public TranssmartMapper Mappers { get; set; }

        /// <summary>
        /// Gets or sets default measurements when not supplied
        /// </summary>
        public Measurements DefaultMeasurements { get; set; }

        /// <summary>
        /// Gets or sets override for quantity uom for line items
        /// </summary>
        public string OverrideQuantityUOM { get; set; }

        /// <summary>
        /// Gets or sets fulfillment carrier id
        /// </summary>
        public Guid FulfillmentCarrierId { get; set; }

        /// <summary>
        /// Gets or sets the mail type for this package
        /// </summary>
        public string MailType { get; set; }

        /// <summary>
        /// Gets or sets the quotes to confirm
        /// </summary>
        public List<FulfillmentCarrierQuoteToConfirm> QuotesToConfirm { get; set; }
    }
}
