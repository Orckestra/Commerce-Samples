using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Transsmart fulfillment carrier quote context
    /// </summary>
    public class TranssmartFulfillmentCarrierQuoteContext
    {
        /// <summary>
        /// Gets or sets the mappers for dealing with conversion to properties
        /// </summary>
        public TranssmartMapper Mappers { get; set; }

        /// <summary>
        /// Gets or sets the default measurements for Transsmart if none provided
        /// </summary>
        public Measurements DefaultMeasurements { get; set; }

        /// <summary>
        /// Gets or sets the address from which the shipment starts
        /// </summary>
        public FulfillmentCarrierAddress AddressFrom { get; set; }

        /// <summary>
        /// Gets or sets the address to which the shipment ends
        /// </summary>
        public FulfillmentCarrierAddress AddressTo { get; set; }

        /// <summary>
        /// Gets or sets the package
        /// </summary>
        public FulfillmentCarrierPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the mail type
        /// </summary>
        public string MailType { get; set; }

        /// <summary>
        /// Gets or sets whether this is a return
        /// </summary>
        public bool IsReturn { get; set; }

        /// <summary>
        /// Gets or sets the carrier service to utilze for the quote
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the culture (language) in which the quote is being processed
        /// </summary>
        public string CultureName { get; set; }
    }
}
