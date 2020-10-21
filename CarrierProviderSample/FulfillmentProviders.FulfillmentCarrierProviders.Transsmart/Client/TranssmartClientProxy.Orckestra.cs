using Orckestra.Overture.ServiceModel.Orders.Fulfillment.Carriers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Transsmart.Client.Model;

namespace Transsmart.Client
{
    /// <summary>
    /// Transsmart client proxy
    /// </summary>
    public partial class TranssmartClientProxy : ITranssmartClientProxy
    {
        /// <summary>
        /// Get rates from Transsmart and return quotes back to caller
        /// </summary>
        /// <param name="context">The context for quotes</param>
        /// <returns>the results</returns>
        public async Task<FulfillmentCarrierQuoteResult> GetRates(TranssmartFulfillmentCarrierQuoteContext context)
        {
            var result = new FulfillmentCarrierQuoteResult
            {
                Quotes = new List<FulfillmentCarrierQuote>(),
                Messages = new List<FulfillmentCarrierQuoteMessage>()
            };

            var rateCalculation = new RateCalculationRequest
            {
                MailType = context.MailType,
                Addresses = new Address[]
                {
                    ConvertAddress(context.AddressFrom, TranssmartAddressType.SEND.ToString()),
                    ConvertAddress(context.AddressTo,  TranssmartAddressType.RECV.ToString())
                },
                Service = context.Service,
                NumberOfPackages = 1,
                Packages = new Package[] { ConvertPackageReduced(context.Mappers, context.Package, context.DefaultMeasurements) }
            };
            var rateResults = await RateCalculation(rateCalculation).ConfigureAwaitWithCulture(false);

            foreach (var rate in rateResults.Rates)
            {
                var quoteInfo = new TranssmartQuoteInfo
                {
                    Language = ExtractLanguageFromCultureName(context.CultureName),
                    Reference = context.Package.PackageId,
                    Carrier = rate.Carrier,
                    CarrierService = rateResults.ShipmentDetails.Service,
                    CarrierServiceOther = rate.ServiceLevelOther,
                    CarrierServiceTime = rate.ServiceLevelTime
                };

                result.Quotes.Add
                (
                    new FulfillmentCarrierQuote
                    {
                        Cost = rate.Price,
                        Currency = rate.Currency,
                        ExpectedDeliveryDate = rate.DeliveryDate,
                        IsReturn = false,
                        FulfillmentCarrierName = rate.Carrier,
                        FulfillmentCarrierService = rate.ServiceLevelTime,
                        QuoteId = CreateTranssmartQuoteInfo(quoteInfo),
                        FulfillmentCarrierImages = rate.CarrierImage == null ? new List<FulfillmentCarrierImage>() : new List <FulfillmentCarrierImage> { rate.CarrierImage }
                    }
                );
            }

            return result;
        }

        /// <summary>
        /// Delete transaction calls to remove shipment from Transsmart system (refund)
        /// </summary>
        /// <param name="quoteId">The quote id info</param>
        /// <returns>completed task</returns>
        public async Task DeleteTransactionAsync(string quoteId)
        {
            try
            {
                var quoteInfo = GetTranssmartQuoteInfo(quoteId);
                await ShipmentDelete(quoteInfo.Reference).ConfigureAwaitWithCulture(false);
            }
            catch (TranssmartException ex)
            {
                _logger.Warn($"Exception occurred trying to delete quote id info: {quoteId}");
                _logger.Warn($"Exception is: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Get shipment documents for a quote id
        /// </summary>
        /// <param name="quoteId">quote id info to use</param>
        /// <returns>List of carrier documents</returns>
        public async Task<List<FulfillmentCarrierDocument>> GetShipmentDocuments(string quoteId)
        {
            var quoteInfo = GetTranssmartQuoteInfo(quoteId);
            var shipment = await GetShipment(quoteInfo.Reference).ConfigureAwaitWithCulture(false);
            var docs = await GetDocuments(quoteInfo.Reference).ConfigureAwaitWithCulture(false);
            return await ConvertShipmentDocuments(docs?.PackageDocs, quoteId, shipment.TrackingAndTraceUrl).ConfigureAwaitWithCulture(false);
        }

        /// <summary>
        /// Confirm quotes with Transsmart (books package)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>list of confirmation <see cref="FulfillmentCarrierQuoteConfirmation"/></returns>
        public async Task<List<FulfillmentCarrierQuoteConfirmation>> ConfirmQuotes(TranssmartFulfillmentCarrierConfirmQuoteContext context)
        {
            var quoteConfirmations = new List<FulfillmentCarrierQuoteConfirmation>();

            try
            {
                foreach (var quote in context.QuotesToConfirm)
                {
                    var quoteInfo = GetTranssmartQuoteInfo(quote.QuoteId);
                    var bookRequest = new BookRequest
                    {
                        Reference = quote.Package.PackageId,
                        CarrierCode = quoteInfo.Carrier,
                        MailType = int.Parse(context.MailType),
                        Language = CorrectLanguage(quoteInfo.Language),
                        Value = quote.Package.Items.Sum(i => i.ValueAmount),
                        ValueCurrency = quote.Package.Items?[0]?.ValueCurrency,
                        Service = quoteInfo.CarrierService,
                        ServiceLevelTime = quoteInfo.CarrierServiceTime,
                        ServiceLevelOther = quoteInfo.CarrierServiceOther,
                        Addresses = new Address[]
                        {
                            ConvertAddress(quote.AddressFrom, TranssmartAddressType.SEND.ToString()),
                            ConvertAddress(quote.AddressTo,  TranssmartAddressType.RECV.ToString())
                        },
                        Inbound = 0,
                        NumberOfPackages = 1,
                        Packages = new Package[] { ConvertPackage(context.Mappers, quote.Package, context.DefaultMeasurements, context.OverrideQuantityUOM) }
                    };
                    var result = await Book(bookRequest).ConfigureAwaitWithCulture(false);
                    var shipmentDocs = await GetShipmentDocuments(quote.QuoteId).ConfigureAwaitWithCulture(false);
                    var quoteConfirmation = new FulfillmentCarrierQuoteConfirmation
                    {
                        QuoteId = quote.QuoteId,
                        FulfillmentCarrierId = context.FulfillmentCarrierId,
                        Documents = shipmentDocs
                    };

                    quoteConfirmations.Add(quoteConfirmation);
                }
            }
            catch (Exception)
            {
                // Refund any other quotes that were confirmed, they need to be batch confirmed successfully
                var confirmedQuotesToRefund = new List<Task>();
                foreach (var confirmedQuote in quoteConfirmations)
                {
                    confirmedQuotesToRefund.Add(DeleteTransactionAsync(confirmedQuote.QuoteId));
                }
                await Task.WhenAll(confirmedQuotesToRefund).ConfigureAwaitWithCulture(false);
                throw;
            }

            return quoteConfirmations;
        }

        /// <summary>
        /// Get Transsmart quote information
        /// </summary>
        /// <param name="jsonString">string created with CreateTranssmartQuoteInfo</param>
        /// <returns>quote info <see cref="TranssmartQuoteInfo"/></returns>
        public TranssmartQuoteInfo GetTranssmartQuoteInfo(string jsonString)
        {
            return Deserialize<TranssmartQuoteInfo>(jsonString);
        }

        /// <summary>
        /// Create Transsmart quote information in json format
        /// </summary>
        /// <param name="quoteInfo">Transsmart quote info</param>
        /// <returns>json string of quote information</returns>
        public string CreateTranssmartQuoteInfo(TranssmartQuoteInfo quoteInfo)
        {
            return Serialize<TranssmartQuoteInfo>(quoteInfo, false);
        }

        private async Task<List<FulfillmentCarrierDocument>> ConvertShipmentDocuments(PackageDocument[] packageDocs, string quoteId, string trackingUrl)
        {

            var results = new List<FulfillmentCarrierDocument>();
            var quoteInfo = GetTranssmartQuoteInfo(quoteId);
            foreach (var doc in packageDocs)
            {
                Func<Task<string>> getCarrierDocument = async () => { return await Task.FromResult(doc.DocumentData).ConfigureAwaitWithCulture(false); };
                var carrierDocumentKey = $"{quoteInfo.Reference}/{Guid.NewGuid().ToString("N")}.{doc.FileFormat}";
                var carrierDataUrl = await _carrierDataCacheManager.GetCarrierDocumentUrl(carrierDocumentKey, getCarrierDocument, doc.FileFormat).ConfigureAwaitWithCulture(false);

                results.Add
                (
                    new FulfillmentCarrierDocument
                    {
                        DocumentFormat = doc.FileFormat,
                        QuoteId = quoteId,
                        LabelUrl = carrierDataUrl,
                        TrackingNumber = doc.AirwayBillNumber,
                        TrackingUrl = trackingUrl
                    }
                );

            }
            return results;
        }

        private static Address ConvertAddress(FulfillmentCarrierAddress address, string type)
        {
            var name = $"{address.FirstName} {address.LastName}";
            return new Address
            {
                Type = type,
                AddressLine1 = address.Line1,
                AddressLine2 = address.Line2,
                Name = name,
                City = address.City,
                Contact = name,
                Country = address.CountryCode,
                Email = address.Email,
                IsResidential = address.IsResidential ? 1 : 0,
                TelephoneNumber = address.PhoneNumber + (string.IsNullOrWhiteSpace(address.PhoneExtension) ? "" : $", Ext. {address.PhoneExtension}"),
                State = address.RegionCode,
                ZipCode = address.PostalCode
            };
        }

        private static Package ConvertPackageReduced(TranssmartMapper mapper, FulfillmentCarrierPackage fulfillmentPackage, Measurements defaultMeasurements)
        {
            var package = new Package
            {
                PackageType = mapper.PackageType.GetMappedValue(fulfillmentPackage.PackageType),
                Measurements = new Measurements
                {
                    Height = fulfillmentPackage.Height,
                    Width = fulfillmentPackage.Width,
                    Length = fulfillmentPackage.Length,
                    LinearUom = mapper.LinearUom.GetMappedValue(fulfillmentPackage.DistanceUOM),
                    Weight = fulfillmentPackage.Weight,
                    MassUom = mapper.WeightUom.GetMappedValue(fulfillmentPackage.WeightUOM),
                },
                Quantity = Convert.ToInt32(fulfillmentPackage.Items.Select(i => i.Quantity).Sum()),
            };

            ApplyDefaultMeasurementsAsNeeded(defaultMeasurements, package.Measurements);
            return package;
        }

        private static Package ConvertPackage(TranssmartMapper mapper, FulfillmentCarrierPackage packageSource, Measurements defaultMeasurements, string overrideQuantityUOM)
        {
            var package = new Package
            {
                LineNo = 1,
                PackageType = mapper.PackageType.GetMappedValue(packageSource.PackageType),
                Measurements = new Measurements
                {
                    Height = packageSource.Height,
                    Width = packageSource.Width,
                    Length = packageSource.Length,
                    LinearUom = mapper.LinearUom.GetMappedValue(packageSource.DistanceUOM),
                    Weight = packageSource.Weight,
                    MassUom = mapper.WeightUom.GetMappedValue(packageSource.WeightUOM),
                },
                Quantity = Convert.ToInt32(packageSource.Items.Select(i => i.Quantity).Sum()),
            };
            ApplyDefaultMeasurementsAsNeeded(defaultMeasurements, package.Measurements);

            if (packageSource.Items?.Count > 0)
            {
                package.DeliveryNoteInfo = new DeliveryNoteInformation
                {
                    DeliveryNoteId = Guid.NewGuid().ToString("N"),
                    Currency = packageSource.Items[0].ValueCurrency,
                    Price = packageSource.Items.Sum(i => i.ValueAmount)
                };
                var lineList = new List<DeliveryNoteLine>();
                foreach(var line in packageSource.Items)
                {
                    var deliveryLine = new DeliveryNoteLine
                    {
                        ArticleName = line.Name,
                        ArticleEanCode = line.Sku,
                        Quantity = Convert.ToInt32(line.Quantity),
                        QuantityUom = overrideQuantityUOM ?? mapper.QuantityUom.GetMappedValue(line.UnitOfMeasure),
                        CountryOrigin = line.CountryCodeOfOrigin,
                        Currency = line.ValueCurrency,
                        Price = line.ValueAmount,
                        GrossWeight = line.Weight > 0 ? line.Weight : defaultMeasurements.Weight,
                        WeightUom = mapper.WeightUom.GetMappedValue(line.WeightUOM)
                    };
                    ApplyDefaultMeasurementsAsNeeded(defaultMeasurements, deliveryLine);

                    lineList.Add(deliveryLine);
                }
                package.DeliveryNoteInfo.DeliveryNoteLines = lineList.ToArray();
            }

            return package;
        }

        private static string CorrectLanguage(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return value.Substring(0, Math.Min(value.Length, 2));
        }

        private static void ApplyDefaultMeasurementsAsNeeded(Measurements defaultMeasurements, Measurements measurements)
        {
            if (defaultMeasurements.Height > 0 && measurements.Height == 0)
            {
                measurements.Height = defaultMeasurements.Height;
            }
            if (defaultMeasurements.Width > 0 && measurements.Width == 0)
            {
                measurements.Width = defaultMeasurements.Width;
            }
            if (defaultMeasurements.Length > 0 && measurements.Length == 0)
            {
                measurements.Length = defaultMeasurements.Length;
            }
            if (defaultMeasurements.Weight > 0 && measurements.Weight == 0)
            {
                measurements.Weight = defaultMeasurements.Weight;
            }
            if (!string.IsNullOrWhiteSpace(defaultMeasurements.LinearUom) && string.IsNullOrWhiteSpace(measurements.LinearUom))
            {
                measurements.LinearUom = defaultMeasurements.LinearUom;
            }
            if (!string.IsNullOrWhiteSpace(defaultMeasurements.MassUom) && string.IsNullOrWhiteSpace(measurements.MassUom))
            {
                measurements.MassUom = defaultMeasurements.MassUom;
            }
        }

        private static void ApplyDefaultMeasurementsAsNeeded(Measurements defaultMeasurements, DeliveryNoteLine deliveryLine)
        {
            if (defaultMeasurements.Weight > 0 && deliveryLine.GrossWeight == 0)
            {
                deliveryLine.GrossWeight = defaultMeasurements.Weight;
            }
            if (!string.IsNullOrWhiteSpace(defaultMeasurements.MassUom) && string.IsNullOrWhiteSpace(deliveryLine.WeightUom))
            {
                deliveryLine.WeightUom = defaultMeasurements.MassUom;
            }
        }

        private string ExtractLanguageFromCultureName(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return null;
            }

            // Transsmart only needs the first part, the language from culture name
            string[] cultureNameParts = cultureName.Split('-');
            return cultureNameParts[0];
        }
    }
}
