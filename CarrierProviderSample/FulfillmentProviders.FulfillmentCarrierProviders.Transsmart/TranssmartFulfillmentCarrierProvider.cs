using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orckestra.Overture.Extensibility.Interfaces;
using Orckestra.Overture.Logging;
using Orckestra.Overture.Providers;
using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using Orckestra.Overture.ServiceModel.Validation;
using Transsmart.Client.Model;
using Transsmart;
using Orckestra.Overture.ServiceModel;
using OrckestraCommerce.FulfillmentProviders.Core.Manager;
using OrckestraCommerce.FulfillmentProviders.Core.Interfaces;
using Newtonsoft.Json;
using OrckestraCommerce.FulfillmentProviders.Core.Factory;
using FulfillmentProviders.FulfillmentCarrierProviders.Transsmart.Resources;
using Transsmart.Client;

namespace OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart
{
    [ProviderImplementation(DisplayName = "ProviderDisplayName", Description = "ProviderDescription", ResourceType = typeof(TranssmartFulfillmentCarrierProviderResources))]
    public class TranssmartFulfillmentCarrierProvider : ProviderBase, IFulfillmentCarrierProvider, ICarrierBookingStrategyAware
    {
        private WebExceptionRetryManager _retryManager;
        private readonly ILog _logger;
        private readonly ITranssmartTokenProvider _cacheVault;
        private readonly ICarrierDataCacheManager _carrierDataCacheManager;
        private readonly ICarrierBookingStrategyFactory _carrierBookingStrategyFactory;
        private IServiceLevelMapperBookingStrategy _serviceLevelMapperBookingStrategy = null; 
        private ITranssmartClientProxy _client;

        public TranssmartFulfillmentCarrierProvider(ITranssmartTokenProvider cacheVault, ILog logger, ICarrierDataCacheManager carrierDataCacheManager,
                                                    ICarrierBookingStrategyFactory carrierBookingStrategyFactory)
        {
            if (cacheVault == null) throw new ArgumentNullException(nameof(cacheVault));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (carrierDataCacheManager == null) throw new ArgumentNullException(nameof(carrierDataCacheManager));
            if (carrierBookingStrategyFactory == null) throw new ArgumentNullException(nameof(carrierBookingStrategyFactory));

            _logger = logger;
            _cacheVault = cacheVault;
            _carrierDataCacheManager = carrierDataCacheManager;
            _carrierBookingStrategyFactory = carrierBookingStrategyFactory;

        }

        private WebExceptionRetryManager RetryManager
        {
            get
            {
                return _retryManager ?? (_retryManager = new WebExceptionRetryManager(_logger));
            }
        }

        public ITranssmartClientProxy TranssmartClient
        {
            get
            {
                if (_client == null)
                {
                    _client = new TranssmartClientProxy(_cacheVault, ApiEndpoint, ApiUsername, ApiPassword, Account, TimeoutInSeconds, _logger,
                                                        DefaultMeasurements, _carrierDataCacheManager);
                }
                return _client;
            }
        }

        public ICarrierBookingStrategy CarrierBookingStrategy
        {
            get
            {
                if (_serviceLevelMapperBookingStrategy == null)
                {
                    _serviceLevelMapperBookingStrategy = _carrierBookingStrategyFactory.BuildCarrierBookingStrategy<IServiceLevelMapperBookingStrategy>((x) =>
                    {
                        try
                        {
                            x.CarrierMappingConfiguration = new Dictionary<string, Dictionary<string, List<string>>>(StringComparer.InvariantCultureIgnoreCase);
                            JsonConvert.PopulateObject(CarrierMappingConfiguration, x.CarrierMappingConfiguration);
                        }
                        catch (Exception ex)
                        {
                            x.CarrierMappingConfiguration = null;
                            throw new NotSupportedException($"The mapping configuration for carriers and service levels must be valid: {CarrierMappingConfiguration}", ex);
                        }
                    });
                }

                return _serviceLevelMapperBookingStrategy;
            }
        }

        public Measurements DefaultMeasurements
        {
            get
            {
                return new Measurements
                {
                    Height = Math.Round(Convert.ToDecimal(DefaultHeightWhenNotSpecified), 2),
                    Width = Math.Round(Convert.ToDecimal(DefaultWidthWhenNotSpecified), 2),
                    Length = Math.Round(Convert.ToDecimal(DefaultLengthWhenNotSpecified), 2),
                    Weight = Math.Round(Convert.ToDecimal(DefaultWeightWhenNotSpecified), 2),
                    LinearUom = DefaultLinearUOMWhenNotSpecified,
                    MassUom = DefaultWeightUOMWhenNotSpecified
                };
            }
        }

        /// <summary>
        /// Gets the UOM mappers from the provider
        /// </summary>
        public TranssmartMapper Mappers
        {
            get
            {
                return new TranssmartMapper(MapperLinearUOM, MapperWeightUOM, MapperQuantityUOM, MapperPackageType);
            }
        }

        /// <summary>
        /// Gets or sets the Transsmart api endpoint
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "ApiEndpoint", DefaultValue = "https://accept-api.transsmart.com", GroupId ="ApiData")]
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the Transsmart api username
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "ApiUsername", GroupId = "ApiData")]
        public string ApiUsername { get; set; }

        /// <summary>
        /// Gets or sets the Transsmart api password
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "ApiPassword", GroupId = "ApiData")]
        public string ApiPassword { get; set; }

        /// <summary>
        /// Gets or sets the Transsmart account to use for API requests
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "Account", GroupId = "ApiData")]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the weight to use when a package's weight is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultWeightWhenNotSpecified", DefaultValue = 0, GroupId = "Measurements")]
        public double DefaultWeightWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the weight unit of measure to use when a package's weight unit of measure is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultWeightUOMWhenNotSpecified", DefaultValue = "KG", GroupId = "Measurements")]
        public string DefaultWeightUOMWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the height to use when a package's height is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultHeightWhenNotSpecified", DefaultValue = 0, GroupId = "Measurements")]
        public double DefaultHeightWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the width to use when a package's width is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultWidthWhenNotSpecified", DefaultValue = 0, GroupId = "Measurements")]
        public double DefaultWidthWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the length to use when a package's length is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultLengthWhenNotSpecified", DefaultValue = 0, GroupId = "Measurements")]
        public double DefaultLengthWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the linear unit of measure to use when a package's height or width unit of measure is not defined.
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "DefaultLinearUOMWhenNotSpecified", DefaultValue = "CM", GroupId = "Measurements")]
        public string DefaultLinearUOMWhenNotSpecified { get; set; }

        /// <summary>
        /// Gets or sets the override quantity uom to use for line items
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "OverrideQuantityUOM", DefaultValue = "PCS", GroupId = "Override")]
        public string OverrideQuantityUOM { get; set; }

        /// <summary>
        /// Gets or sets the default service to use (Default of NON-DOCS)
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "ServiceType", DefaultValue = "NON-DOCS", GroupId = "Override")]
        public string ServiceType { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds to use for the API
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "TimeoutInSeconds", DefaultValue = 30)]
        public int TimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the mail type to use for booking
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "MailType", DefaultValue = "1", GroupId = "Override")]
        public string MailType { get; set; }

        /// <summary>
        /// Gets or sets the mapper for linear uom
        /// </summary>
        [ProviderProperty(IsRequired = false, DisplayName = "MapperLinearUOM", DefaultValue = "", GroupId = "Mapper")]
        public string MapperLinearUOM { get; set; }

        /// <summary>
        /// Gets or sets the mapper for package type
        /// </summary>
        [ProviderProperty(IsRequired = false, DisplayName = "MapperPackageType", DefaultValue = "", GroupId = "Mapper")]
        public string MapperPackageType { get; set; }

        /// <summary>
        /// Gets or sets the mapper for weight uom
        /// </summary>
        [ProviderProperty(IsRequired = false, DisplayName = "MapperWeightUOM", DefaultValue = "", GroupId = "Mapper")]
        public string MapperWeightUOM { get; set; }

        /// <summary>
        /// Gets or sets the mapper for quantity uom
        /// </summary>
        [ProviderProperty(IsRequired = false, DisplayName = "MapperQuantityUOM", DefaultValue = "", GroupId = "Mapper")]
        public string MapperQuantityUOM { get; set; }

        /// <summary>
        /// Gets or sets the residential flag for addresses
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "IsResidential", DefaultValue = true, GroupId = "Override")]
        public bool IsResidential { get; set; }

        /// <summary>
        /// Gets or sets the service level mappings informations
        /// </summary>
        [ProviderProperty(IsRequired = true, DisplayName = "CarrierMappingConfiguration", DefaultValue = "", GroupId = "Override")]
        public string CarrierMappingConfiguration { get; set; }

        public Task<FulfillmentCarrierQuoteResult> GetQuotes(CalculateFulfillmentCarrierContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Package == null) throw new ArgumentNullException(nameof(context.Package));
            if (context.Package.PackageId == null) throw new ArgumentNullException(nameof(context.Package.PackageId));
            if (context.AddressFrom == null) throw new ArgumentNullException(nameof(context.AddressFrom));
            if (context.AddressTo == null) throw new ArgumentNullException(nameof(context.AddressTo));
            
            var quoteContext = new TranssmartFulfillmentCarrierQuoteContext
            {
                Mappers = Mappers,
                MailType = MailType,
                DefaultMeasurements = DefaultMeasurements,
                AddressFrom = context.AddressFrom,
                AddressTo = context.AddressTo,
                IsReturn = context.IsReturn,
                Package = context.Package,
                Service = ServiceType,
                CultureName = context.CultureName
            };

            // Residential address types will be examined in the near future, for now, set the default
            quoteContext.AddressFrom.IsResidential = IsResidential;
            quoteContext.AddressTo.IsResidential = IsResidential;

            return RetryManager.ExecuteAsync(() => TranssmartClient.GetRates(quoteContext));
        }

        /// <summary>
        /// Refund shipping quotes with the carrier
        /// </summary>
        /// <param name="context">The context containing the quote to refund</param>
        /// <returns>An awaitable task</returns>
        public Task RefundQuotes(FulfillmentCarrierQuoteRefundContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Quotes == null) throw new ArgumentNullException(nameof(context.Quotes));

            if (context.Quotes.Count == 0)
                return Task.CompletedTask;

            if (context.Quotes.Any(quoteToRefund => string.IsNullOrWhiteSpace(quoteToRefund.QuoteId)))
                throw new InvalidOperationException("QuoteId is required to refund a quote.");

            var refundTasks = context.Quotes.Select
            (quoteToRefund => 
                RetryManager.ExecuteAsync(() =>
                    TranssmartClient.DeleteTransactionAsync(quoteToRefund.QuoteId))
            );
            return Task.WhenAll(refundTasks);
        }

        public async Task<List<FulfillmentCarrierDocument>> GetDocuments(FulfillmentCarrierDocumentsRetrievalContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.DocumentsToRetrieve == null) throw new ArgumentNullException(nameof(context.DocumentsToRetrieve));
            if (context.DocumentsToRetrieve.Count == 0) throw new ArgumentException("Should not be empty", nameof(context.DocumentsToRetrieve));

            var documents = new List<FulfillmentCarrierDocument>();
            foreach (var documentToRetrieve in context.DocumentsToRetrieve)
            {
                if (String.IsNullOrWhiteSpace(documentToRetrieve.QuoteId))
                {
                    throw new ArgumentException(nameof(FulfillmentCarrierDocumentToRetrieve.QuoteId) + " cannot be null, empty or whitespace",
                                                nameof(FulfillmentCarrierDocumentToRetrieve.QuoteId));
                }

                var docs = await TranssmartClient.GetShipmentDocuments(documentToRetrieve.QuoteId).ConfigureAwaitWithCulture(false);
                if (docs == null || docs.Count == 0)
                {
                    throw new InvalidOperationException($"Transsmart shipment documents for reference id {documentToRetrieve.QuoteId} were not found");
                }
                documents.AddRange(docs);
            }
            return documents;
        }

        public Task<List<FulfillmentCarrierQuoteConfirmation>> ConfirmQuotes(FulfillmentCarrierQuoteConfirmationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context.Quotes == null) throw new ArgumentNullException(nameof(context.Quotes));

            var quoteContext = new TranssmartFulfillmentCarrierConfirmQuoteContext
            {
                Mappers = Mappers,
                OverrideQuantityUOM = string.IsNullOrWhiteSpace(OverrideQuantityUOM) ? null : OverrideQuantityUOM,
                DefaultMeasurements = DefaultMeasurements,
                FulfillmentCarrierId = Id,
                MailType = MailType,
                QuotesToConfirm = context.Quotes
            };

            // Residential address types will be examined in the near future, for now, set the default
            foreach (var quote in quoteContext.QuotesToConfirm)
            {
                quote.AddressFrom.IsResidential = IsResidential;
                quote.AddressTo.IsResidential = IsResidential;
            }

            return RetryManager.ExecuteAsync(() => TranssmartClient.ConfirmQuotes(quoteContext));
        }

        public Task<IEnumerable<ValidationResult>> ValidateSettings()
        {
            var results = new List<ValidationResult>();

            if (DefaultMeasurements.Height <= 0)
            {
                AddValidationError(results, "DefaultHeightMeasumentContainsZero",
                    $"The default measurement for height should be greater than 0: {nameof(DefaultHeightWhenNotSpecified)}");
            }

            if (DefaultMeasurements.Width <= 0)
            {
                AddValidationError(results, "DefaultWidthMeasumentContainsZero",
                    $"The default measurement for width should be greater than 0: {nameof(DefaultWidthWhenNotSpecified)}");
            }

            if (DefaultMeasurements.Length <= 0)
            {
                AddValidationError(results, "DefaultLengthMeasumentContainsZero",
                    $"The default measurement for length should be greater than 0: {nameof(DefaultLengthWhenNotSpecified)}");
            }

            if (!(DefaultMeasurements.Weight > 0))
            {
                AddValidationError(results, "DefaultWeightMeasumentContainsZero",
                    $"The default measurement for weight should be greater than 0: {nameof(DefaultWeightWhenNotSpecified)}");
            }

            if (string.IsNullOrWhiteSpace(ApiEndpoint))
            {
                AddValidationError(results, "ApiEndPointConnectionInfoMissing",
                    $"The Transsmart api end point is required: {nameof(ApiEndpoint)}");
            }

            if (string.IsNullOrWhiteSpace(ApiUsername))
            {
                AddValidationError(results, "ApiUsernameConnectionInfoMissing",
                    $"The Transsmart api username is required: {nameof(ApiUsername)}");
            }

            if (string.IsNullOrWhiteSpace(ApiPassword))
            {
                AddValidationError(results, "ApiPasswordConnectionInfoMissing",
                    $"The Transsmart api password is required: {nameof(ApiPassword)}");
            }

            if (string.IsNullOrWhiteSpace(Account))
            {
                AddValidationError(results, "AccountNotSet", $"The Transsmart account needs to be supplied: : {nameof(Account)}");
            }

            if (string.IsNullOrWhiteSpace(MailType))
            {
                AddValidationError(results, "MailTypeNotSet", $"The Transsmart mail type needs to be supplied: : {nameof(MailType)}");
            }

            if (string.IsNullOrWhiteSpace(ServiceType))
            {
                AddValidationError(results, "ServiceTypeNotSet", $"The Transsmart service type needs to be supplied: : {nameof(ServiceType)}");
            }
            else if (!TranssmartOptions.ValidServiceTypes.Contains(ServiceType))
            {
                AddValidationError(results, "ServiceTypeNotValid", $"The Transsmart service type needs to be valid: : {nameof(ServiceType)}={ServiceType}, valid values are {string.Join(", ", TranssmartOptions.ValidServiceTypes)}");
            }

            if (!TranssmartOptions.ValidLinearUOM.Contains(DefaultLinearUOMWhenNotSpecified))
            {
                AddValidationError(results, "DefaultLinearUOMWhenNotSpecifiedNotValid", $"The Transsmart default linear uom needs to be valid: : {nameof(DefaultLinearUOMWhenNotSpecified)}={DefaultLinearUOMWhenNotSpecified}, " +
                    $"valid values are {string.Join(", ", TranssmartOptions.ValidLinearUOM)}");
            }

            if (!TranssmartOptions.ValidMassUOM.Contains(DefaultWeightUOMWhenNotSpecified))
            {
                AddValidationError(results, "DefaultWeightUOMWhenNotSpecifiedNotValid", $"The Transsmart default weight uom needs to be valid: : {nameof(DefaultWeightUOMWhenNotSpecified)}={DefaultWeightUOMWhenNotSpecified}, " +
                    $"valid values are {string.Join(", ", TranssmartOptions.ValidMassUOM)}");
            }

            if (TimeoutInSeconds < 1)
            {
                AddValidationError(results, "TimeoutInSecondsNotValid", $"The Transsmart timeout in seconds is not valid: {nameof(TimeoutInSeconds)}={TimeoutInSeconds}, needs to be greater than 0");
            }

            try
            {
                var carrierMappingConfiguration = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(CarrierMappingConfiguration);
            }
            catch (Exception ex)
            {
                AddValidationError(results, "CarrierMappingConfiguration", $"The mapping configuration for carriers and service levels must be valid. Error: {ex.Message}");
            }

            try
            {
                var mappers = Mappers;
            }
            catch(Exception ex)
            {
                AddValidationError(results, "UOMMappersNotValid", $"The Transsmart UOM mappers (MapperLinearUOM, MapperWeightUOM, MapperQuantityUOM) need to be valid. Error: {ex.Message}");
            }
            return Task.FromResult<IEnumerable<ValidationResult>>(results);
        }

        private void AddValidationError(List<ValidationResult> results, string errorCode, string errorMessage)
        {
            results.Add(new ValidationResult
            {
                Category = ValidationErrorCategories.Fulfillment,
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        Context = new PropertyBag {{ "context.TranssmartFulfillmentCarrierProvider", this}},
                        Descriptor = new ValidationFailureDescriptor
                        {
                            ErrorCode = errorCode,
                            ErrorMessage = errorMessage
                        },
                        EntityType = "TranssmartFulfillmentCarrierProvider",
                        Severity = ValidationFailureSeverity.Error
                    }
                }
            });
        }
    }
}
