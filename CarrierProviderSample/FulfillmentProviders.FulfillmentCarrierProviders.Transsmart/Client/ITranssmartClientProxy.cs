using System.Drawing;
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
        /// Gets the api endpoint
        /// </summary>
        string ApiEndPoint { get; }

        /// <summary>
        /// Gets the api username
        /// </summary>
        string ApiUsername { get; }

        /// <summary>
        /// Gets the api password
        /// </summary>
        string ApiPassword { get; }

        /// <summary>
        /// Gets the api account
        /// </summary>
        string ApiAccount { get; }

        /// <summary>
        /// Get Token for accessing Transsmart
        /// </summary>
        /// <returns>the token results</returns>
        Task<GetTokenResult> GetToken();

        /// <summary>
        /// Book and print a shipment (documents produced)
        /// </summary>
        /// <param name="requestBookAndPrint">the request</param>
        /// <returns>the book and print results <see cref="BookAndPrintResult" /></returns>
        Task<BookAndPrintResult> BookAndPrint(BookAndPrintRequest requestBookAndPrint);

        /// <summary>
        /// Book a shipment (GetDocuments can be used to retrieve documents)
        /// </summary>
        /// <param name="requestBook">the book request</param>
        /// <returns>the booking result <see cref="BookResult"/></returns>
        Task<BookResult> Book(BookRequest requestBook);

        /// <summary>
        /// Get print documents for booked shipment
        /// </summary>
        /// <param name="reference">Shipment reference</param>
        /// <returns>Documents <see cref="GetDocumentsResult" /></returns>
        Task<GetDocumentsResult> GetDocuments(string reference);

        /// <summary>
        /// Get shipment data
        /// </summary>
        /// <param name="referenceId">shipment reference id</param>
        /// <returns>shipment data <see cref="Shipment" /></returns>
        Task<Shipment> GetShipment(string referenceId);

        /// <summary>
        /// Delete a shipment
        /// </summary>
        /// <param name="reference">the shipment reference to delete</param>
        /// <returns>true for success, an exception for an error</returns>
        Task<bool> ShipmentDelete(string reference);

        /// <summary>
        /// Rate calculation will be processed
        /// </summary>
        /// <param name="rateCalculationRequest">the request with addresses</param>
        /// <returns>the rate calculation results <see cref="RateCalculationResult"/></returns>
        Task<RateCalculationResult> RateCalculation(RateCalculationRequest rateCalculationRequest);

        /// <summary>
        /// Retrieve a carrier logo
        /// </summary>
        /// <param name="carrierLogoRequest">The request</param>
        /// <returns>The carrier logo results <see cref="CarrierLogoResult"/></returns>
        Task<CarrierLogoResult> GetCarrierLogo(CarrierLogoRequest carrierLogoRequest);

        /// <summary>
        /// Convert base 64 strimg to an image
        /// </summary>
        /// <param name="base64Image">base 64 encoded image</param>
        /// <returns>image</returns>
        Image GetImageFromBase64(string base64Image);

        /// <summary>
        /// Return image width from a base 64 encoded image
        /// </summary>
        /// <param name="base64Image">base 64 encoded string</param>
        /// <returns>width of image</returns>
        int GetImageWidthFromBase64(string base64Image);
    }
}
