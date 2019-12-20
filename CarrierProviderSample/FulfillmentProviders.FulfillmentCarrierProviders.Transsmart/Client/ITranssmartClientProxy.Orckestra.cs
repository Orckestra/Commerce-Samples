using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transsmart.Client.Model;

namespace Transsmart.Client
{
    /// <summary>
    /// The client proxy that allows Transsmart v2 API calls to be performed
    /// </summary>
    public partial interface ITranssmartClientProxy
    {
        /// <summary>
        /// Get rates from Transsmart
        /// </summary>
        /// <param name="context">Quote context</param>
        /// <returns>Quote results</returns>
        Task<FulfillmentCarrierQuoteResult> GetRates(TranssmartFulfillmentCarrierQuoteContext context);

        /// <summary>
        /// Delete transaction call to remove shipment from Transsmart system (refund)
        /// </summary>
        /// <param name="quoteId">The quote id info</param>
        /// <returns>completed task</returns>
        Task DeleteTransactionAsync(string quoteId);

        /// <summary>
        /// Get shipment documents for a quote id
        /// </summary>
        /// <param name="quoteId">quote id info to use</param>
        /// <returns>List of carrier documents</returns>
        Task<List<FulfillmentCarrierDocument>> GetShipmentDocuments(string quoteId);

        /// <summary>
        /// Confirm quotes (books shipment) with Transsmart
        /// </summary>
        /// <param name="context">Context for request</param>
        /// <returns>Confirmation of booked shipment</returns>
        Task<List<FulfillmentCarrierQuoteConfirmation>> ConfirmQuotes(TranssmartFulfillmentCarrierConfirmQuoteContext context);

        /// <summary>
        /// Get Transsmart quote information
        /// </summary>
        /// <param name="jsonString">string created with CreateTranssmartQuoteInfo</param>
        /// <returns>quote info <see cref="TranssmartQuoteInfo"/></returns>
        TranssmartQuoteInfo GetTranssmartQuoteInfo(string jsonString);

        /// <summary>
        /// Create Transsmart quote information in json format
        /// </summary>
        /// <param name="quoteInfo">Transsmart quote info</param>
        /// <returns>json string of quote information</returns>
        string CreateTranssmartQuoteInfo(TranssmartQuoteInfo quoteInfo);
    }
}
