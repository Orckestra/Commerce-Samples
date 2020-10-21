using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Orckestra.Overture.Logging;
using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using OrckestraCommerce.FulfillmentProviders.Core.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Transsmart.Client.Model;
using static System.Net.WebRequestMethods;

namespace Transsmart.Client
{
    /// <summary>
    /// Transsmart client proxy
    /// </summary>
    public partial class TranssmartClientProxy : ITranssmartClientProxy
    {
        private readonly static Encoding _encoding = Encoding.UTF8;

        private readonly int _apiTimeoutInSeconds;
        private readonly ICarrierDataCacheManager _carrierDataCacheManager;
        private ITranssmartTokenProvider _tokenProvider;
        private Measurements _defaultMeasurements;
        private ILog _logger;

        /// <summary>
        /// Constructor for proxy
        /// </summary>
        /// <param name="tokenProvider">token provider</param>
        /// <param name="apiEndpoint">api endpoint</param>
        /// <param name="apiUsername">api username</param>
        /// <param name="apiPassword">api password</param>
        /// <param name="apiAccount">api account</param>
        /// <param name="apiTimeoutInSeconds">api timeout in seconds</param>
        /// <param name="logger">logger to use</param>
        /// <param name="defaultMeasurements">default measurments to use</param>
        /// <param name="carrierDataCacheManager">carrier logo manage</param>
        public TranssmartClientProxy(ITranssmartTokenProvider tokenProvider,
            string apiEndpoint,
            string apiUsername,
            string apiPassword,
            string apiAccount,
            int apiTimeoutInSeconds,
            ILog logger,
            Measurements defaultMeasurements,
            ICarrierDataCacheManager carrierDataCacheManager)
        {
            if (tokenProvider == null) throw new ArgumentNullException(nameof(tokenProvider));
            if (string.IsNullOrWhiteSpace(apiEndpoint)) throw new ArgumentNullException(nameof(apiEndpoint));
            if (string.IsNullOrWhiteSpace(apiUsername)) throw new ArgumentNullException(nameof(apiUsername));
            if (string.IsNullOrWhiteSpace(apiPassword)) throw new ArgumentNullException(nameof(apiPassword));
            if (string.IsNullOrWhiteSpace(apiAccount)) throw new ArgumentNullException(nameof(apiAccount));
            if (apiTimeoutInSeconds < 1) throw new ArgumentOutOfRangeException(nameof(apiTimeoutInSeconds), $"The value for {nameof(_apiTimeoutInSeconds)} must be a positive integer");
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (carrierDataCacheManager == null) throw new ArgumentNullException(nameof(carrierDataCacheManager));

            _tokenProvider = tokenProvider;
            ApiEndPoint = apiEndpoint;
            ApiUsername = apiUsername;
            ApiPassword = apiPassword;
            ApiAccount = apiAccount;
            _apiTimeoutInSeconds = apiTimeoutInSeconds;
            _logger = logger;
            _defaultMeasurements = defaultMeasurements ?? new Measurements
            {
                Height = 0,
                Length = 0,
                Width = 0,
                Weight = 0,
                LinearUom = "CM",
                MassUom = "KG"
            };
            _carrierDataCacheManager = carrierDataCacheManager;
        }

        /// <summary>
        /// Gets the api endpoint
        /// </summary>
        public string ApiEndPoint { get; }

        /// <summary>
        /// Gets the api username
        /// </summary>
        public string ApiUsername { get; }


        /// <summary>
        /// Gets the api password
        /// </summary>
        public string ApiPassword { get; }

        /// <summary>
        /// Gets the api account
        /// </summary>
        public string ApiAccount { get; }

        /// <summary>
        /// Get token for using with api
        /// </summary>
        /// <returns>get token result <see cref="GetTokenResult" /></returns>
        public Task<GetTokenResult> GetToken()
        {
            string request = $"{ApiEndPoint}/login";
            return DoRequestAsync<GetTokenResult>(request, Http.Get);
        }

        /// <summary>
        /// Book a request
        /// </summary>
        /// <param name="requestBook">the request for a booking</param>
        /// <returns>a book result <see cref="BookResult" /></returns>
        public Task<BookResult> Book(BookRequest requestBook)
        {
            string request = $"{ApiEndPoint}/v2/shipments/{ApiAccount}/BOOK";
            return DoRequestAsync<BookResult>(request, Http.Post, Serialize(requestBook));
        }

        /// <summary>
        /// Book and print a shipment (documents produced)
        /// </summary>
        /// <param name="requestBookAndPrint">the request</param>
        /// <returns>the book and print results <see cref="BookAndPrintResult" /></returns>
        public Task<BookAndPrintResult> BookAndPrint(BookAndPrintRequest requestBookAndPrint)
        {
            string request = $"{ApiEndPoint}/v2/shipments/{ApiAccount}/PRINT?rawJob=true";
            return DoRequestAsync<BookAndPrintResult>(request, Http.Post, Serialize(requestBookAndPrint));
        }

        /// <summary>
        /// Get print documents for booked shipment
        /// </summary>
        /// <param name="reference">Shipment reference</param>
        /// <returns>Documents <see cref="GetDocumentsResult" /></returns>
        public Task<GetDocumentsResult> GetDocuments(string referenceId)
        {
            string request = $"{ApiEndPoint}/v2/prints/{ApiAccount}/{referenceId}?rawJob=true";
            return DoRequestAsync<GetDocumentsResult>(request, Http.Get);
        }

        /// <summary>
        /// Get shipment data
        /// </summary>
        /// <param name="referenceId">shipment reference id</param>
        /// <returns>shipment data <see cref="Shipment" /></returns>
        public async Task<Shipment> GetShipment(string referenceId)
        {
            string request = $"{ApiEndPoint}/v2/shipments/{ApiAccount}/{referenceId}";
            var resultJson = await DoRequestAsync(request, Http.Get).ConfigureAwaitWithCulture(false);

            var result = Deserialize<Shipment>(resultJson);
            if (result == null || string.IsNullOrWhiteSpace(result.Reference))
            {
                var error = Deserialize<TranssmartError>(resultJson);
                throw new TranssmartException(error.ToString(), null);
            }
            return result;
        }

        /// <summary>
        /// Delete a shipment
        /// </summary>
        /// <param name="reference">the shipment reference to delete</param>
        /// <returns>true for success, an exception for an error</returns>
        public async Task<bool> ShipmentDelete(string reference)
        {
            string request = $"{ApiEndPoint}/v2/shipments/{ApiAccount}/{reference}";
            var result = await DoRequestAsync(request, "DELETE").ConfigureAwaitWithCulture(false);

            // If error, throw it
            if (!string.Equals(result, "true", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Error($"Error deleting shipment reference {reference}, result: {result}");
                throw new TranssmartException($"Error deleting shipment reference {reference}", new TranssmartException(result, null));
            }
            return true;
        }

        /// <summary>
        /// Rate calculation will be processed
        /// </summary>
        /// <param name="rateCalculationRequest">the request with addresses</param>
        /// <returns>the rate calculation results <see cref="RateCalculationResult"/></returns>
        public async Task<RateCalculationResult> RateCalculation(RateCalculationRequest rateCalculationRequest)
        {
            try
            {
                string request = $"{ApiEndPoint}/v2/rates/{ApiAccount}";
                var result = await DoRequestAsync<RateCalculationResult>(request, Http.Post, Serialize(rateCalculationRequest)).ConfigureAwaitWithCulture(false);

                // If error, throw it
                if (result?.Errors != null)
                {
                    _logger.Error($"Error calculating rates, result: {result.Errors.Message}, full error:{result.Errors.ToString()}");
                    _logger.Error($"Request is: {Serialize(rateCalculationRequest)}");
                    throw new TranssmartException($"Error calculating rates, result: {result.Errors.Message}", new TranssmartException(result.Errors.ToString(), null));
                }

                await SetCarrierLogoInRateResult(rateCalculationRequest.Addresses, result).ConfigureAwaitWithCulture(false);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during Rate Calculation, {ex.ToString()}");
                _logger.Error($"Error with request: {Serialize(rateCalculationRequest)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieve a carrier logo
        /// </summary>
        /// <param name="carrierLogoRequest">The request</param>
        /// <returns>The carrier logo results <see cref="CarrierLogoResult"/></returns>
        public async Task<CarrierLogoResult> GetCarrierLogo(CarrierLogoRequest carrierLogoRequest)
        {
            if (carrierLogoRequest == null) throw new ArgumentNullException(nameof(carrierLogoRequest));
            if (string.IsNullOrWhiteSpace(carrierLogoRequest.Carrier)) throw new ArgumentNullException(nameof(carrierLogoRequest.Carrier));
            if (string.IsNullOrWhiteSpace(carrierLogoRequest.ZipCode )) throw new ArgumentNullException(nameof(carrierLogoRequest.ZipCode));
            if (string.IsNullOrWhiteSpace(carrierLogoRequest.CountryFrom)) throw new ArgumentNullException(nameof(carrierLogoRequest.CountryFrom));
            if (string.IsNullOrWhiteSpace(carrierLogoRequest.CountryTo)) throw new ArgumentNullException(nameof(carrierLogoRequest.CountryTo));

            string request = $"{ApiEndPoint}/v2/locations/{ApiAccount}?zipCode={carrierLogoRequest.ZipCode}&countryTo={carrierLogoRequest.CountryTo}&countryFrom={carrierLogoRequest.CountryFrom}&provider={carrierLogoRequest.Carrier}";
            var results = await DoRequestAsync<CarrierLogoResult>(request, Http.Get).ConfigureAwaitWithCulture(false);

            if (results?.Sources != null)
            {
                var errorsFound = new List<TranssmartError>();
                foreach (var source in results.Sources)
                {
                    if (source.Errors?.Message != null)
                    {
                        errorsFound.Add(source.Errors);
                    }
                }

                // If error, throw it
                if (errorsFound.Count > 0)
                {
                    StringBuilder errorCombined = new StringBuilder();
                    errorCombined.Append("Error getting carrier logo, result: ");
                    errorCombined.Append(string.Join(", ", errorsFound.Select(e => e.Message).ToList()));
                    _logger.Error(errorCombined);
                    foreach (var error in errorsFound)
                    {
                        _logger.Error(error.ToString());
                    }
                    throw new TranssmartException($"Error getting carrier logo: {errorCombined}", null);
                }
            }

            return results;
        }

        /// <summary>
        /// Return image width from a base 64 encoded image
        /// </summary>
        /// <param name="base64Image">base 64 encoded string</param>
        /// <returns>width of image</returns>
        public int GetImageWidthFromBase64(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return 0;
            }

            using (var image = GetImageFromBase64(base64Image))
            {
                return image.Width;
            }
        }

        /// <summary>
        /// Get image from a base 64 encoded string
        /// </summary>
        /// <param name="base64Image">base 64 encoded string</param>
        /// <returns>image</returns>
        public Image GetImageFromBase64(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                return null;
            }

            byte[] bytes = Convert.FromBase64String(base64Image);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        private async Task SetCarrierLogoInRateResult(Address[] addresses, RateCalculationResult rateResult)
        {
            if (addresses == null) throw new ArgumentNullException(nameof(addresses));

            var sendAddress = addresses.Where(a => a.Type == TranssmartAddressType.SEND.ToString()).ToList();
            if (sendAddress.Count == 0)
            {
                _logger.Warn($"Transsmart: request does not include address {TranssmartAddressType.SEND}, unable to get carrier logo");
                return;
            }
            else if (sendAddress.Count > 1)
            {
                _logger.Warn($"Transsmart: only one address {TranssmartAddressType.SEND} is supported, unable to get carrier logo");
                return;
            }

            var recAddress = addresses.Where(a => a.Type == TranssmartAddressType.RECV.ToString()).ToList();
            if (recAddress.Count == 0)
            {
                _logger.Warn($"Transsmart: request does not include address {TranssmartAddressType.RECV}, unable to get carrier logo");
                return;
            }
            else if (recAddress.Count > 1)
            {
                _logger.Warn($"Transsmart: only one address {TranssmartAddressType.RECV} is supported, unable to get carrier logo");
                return;
            }

            if (string.IsNullOrWhiteSpace(recAddress[0].ZipCode))
            {
                _logger.Warn($"Transsmart: request address {TranssmartAddressType.RECV} has no zip code, unable to get carrier logo");
                return;
            }

            if (string.IsNullOrWhiteSpace(sendAddress[0].Country))
            {
                _logger.Warn($"Transsmart: request send address is missing a country, unable to get carrier logo");
                return;
            }

            if (string.IsNullOrWhiteSpace(recAddress[0].Country))
            {
                _logger.Warn($"Transsmart: request recieve address is missing a country, unable to get carrier logo");
                return;
            }

            var getCarrierLogoRequest = new CarrierLogoRequest
            {
                CountryFrom = sendAddress[0].Country,
                CountryTo = recAddress[0].Country,
                ZipCode = recAddress[0].ZipCode
            };

            Func<Task<string>> getCarrierLogo = async () =>
            {
                var result = await GetCarrierLogo(getCarrierLogoRequest).ConfigureAwaitWithCulture(false);

                if (result?.Sources.Length > 0 && result.Sources[0]?.CarrierLogo != null)
                {
                    return result.Sources[0].CarrierLogo;
                }

                return null;
            };

            foreach (var rate in rateResult.Rates)
            {
                getCarrierLogoRequest.Carrier = rate.Carrier;

                var carrierLogoKey = $"{rate.Carrier}{rate.ServiceLevelTime}{rate.ServiceLevelOther}{sendAddress[0].Country}{recAddress[0].Country}";

                var carrierLogoUrl = await _carrierDataCacheManager.GetCarrierLogoUrl(carrierLogoKey, getCarrierLogo).ConfigureAwaitWithCulture(false);

                if (string.IsNullOrWhiteSpace(carrierLogoUrl))
                {
                    _logger.Warn($"Transsmart: get carrier logo failed for carrier: {getCarrierLogoRequest.Carrier} zip code:{getCarrierLogoRequest.ZipCode}, country from: {getCarrierLogoRequest.CountryFrom}, country to: {getCarrierLogoRequest.CountryTo}");
                }
                else
                {
                    rate.CarrierImage = new FulfillmentCarrierImage
                    {
                        Url = carrierLogoUrl,
                    };
                }
            }
        }

        private async Task<T> DoRequestAsync<T>(string endpoint, string method = Http.Get, string body = null)
        {
            var json = await DoRequestAsync(endpoint, method, body).ConfigureAwaitWithCulture(false);
            return Deserialize<T>(json);
        }

        private async Task<string> DoRequestAsync(string endpoint, string method, string body = null)
        {
            string result = null;
            try
            {
                WebRequest req = await SetupRequest(method, endpoint).ConfigureAwaitWithCulture(false);
                if (body != null)
                {
                    byte[] bytes = _encoding.GetBytes(body);
                    req.ContentLength = bytes.Length;
                    using (Stream st = await req.GetRequestStreamAsync().ConfigureAwaitWithCulture(false))
                    {
                        await st.WriteAsync(bytes, 0, bytes.Length).ConfigureAwaitWithCulture(false);
                    }
                }

                using (WebResponse resp = (WebResponse)req.GetResponse())
                {
                    result = await GetResponseAsString(resp).ConfigureAwaitWithCulture(false);
                }
            }
            catch (WebException wexc)
            {
                if (wexc.Response != null)
                {
                    string json_error = await GetResponseAsString(wexc.Response).ConfigureAwaitWithCulture(false);
                    HttpWebResponse resp = wexc.Response as HttpWebResponse;
                    HttpStatusCode status_code = resp != null ? resp.StatusCode : HttpStatusCode.BadRequest;
                    wexc.Response.Dispose();
                    if ((int)status_code <= 500)
                        throw new TranssmartException(json_error, wexc);
                }
                _logger.Error($"Error getting response from api {endpoint} with method {method}, web exception: {wexc.ToString()}");
                throw new TranssmartException($"Error getting response from api {endpoint} with method {method}", wexc); ;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting response from api {endpoint} with method {method}, exception: {ex.ToString()}");
                throw new TranssmartException($"Error getting response from api {endpoint} with method {method}", ex); ;
            }
            return result.Trim('[').Trim(']');
        }

        private async Task<WebRequest> SetupRequest(string method, string url)
        {
            WebRequest req = (WebRequest)WebRequest.Create(url);
            req.Method = method;
            if (url.ToLower().EndsWith("/login"))
            {
                req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ApiUsername}:{ApiPassword}"));
            }
            else
            {
                var token = await _tokenProvider.GetToken(this).ConfigureAwaitWithCulture(false);
                req.Headers["Authorization"] = $"Bearer {token}";
                req.Headers["Accept-Encoding"] = "gzip, deflate";
            }

            req.Timeout = _apiTimeoutInSeconds * 1000;
            req.ContentType = "application/json";
            return req;
        }

        private static string Serialize<T>(T data, bool addAsObjectArray = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new StringEnumConverter());
            if (addAsObjectArray)
            {
                return JsonConvert.SerializeObject(new object[] { data }, settings);
            }
            return JsonConvert.SerializeObject(data, settings);
        }

        private static T Deserialize<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData, new TranssmartDateTimeConverter());
        }

        private static async Task<string> GetResponseAsString(WebResponse response)
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), _encoding))
            {
                return await sr.ReadToEndAsync().ConfigureAwaitWithCulture(false);
            }
        }
    }
}
