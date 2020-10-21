using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Orckestra.Overture.Logging;
using Transsmart.Client.Model;

namespace OrckestraCommerce.FulfillmentProviders.FulfillmentCarrierProviders.Transsmart
{
    public class WebExceptionRetryManager
    {
        private readonly ILog _logger;
        protected RetryPolicy RetryPolicy;

        public WebExceptionRetryManager(ILog logger) : base()
        {
            _logger = logger;

            RetryPolicy = GetRetryPolicy();
            RetryPolicy.Retrying += (sender, args) => { LogError(args); };
        }

        private static RetryPolicy GetRetryPolicy()
        {
            try
            {
                return RetryManager.Instance.GetRetryPolicy<CarrierClientStrategy>("Incremental Retry Strategy");
            }
            catch
            {
                return RetryManager.Instance.GetRetryPolicy<CarrierClientStrategy>(RetryManager.Instance.DefaultRetryStrategyName);
            }
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            return RetryPolicy.ExecuteAsync(action);
        }

        public Task<TResponse> ExecuteAsync<TResponse>(Func<Task<TResponse>> action)
        {
            return RetryPolicy.ExecuteAsync(action);
        }

        protected void LogError(RetryingEventArgs args)
        {
            var reason = string.IsNullOrWhiteSpace(args?.LastException?.Message) ? "Unknown" : args.LastException.Message;
            _logger.Error($"Retry condition encountered in WebExceptionRetryManager. MachineName: {Environment.MachineName}: {reason}",
                          args?.LastException);
        }
    }

    internal class CarrierClientStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex is TranssmartException ||
                ex is NotSupportedException)
                return false;

            var webException = ex as WebException;
            if (webException?.Response != null)
            {
                HttpStatusCode statusCode = HttpStatusCode.BadRequest;
                HttpWebResponse resp = webException.Response as HttpWebResponse;
                if (resp != null)
                    statusCode = resp.StatusCode;

                var code = (int) statusCode;
                if (code == (int)HttpStatusCode.NotFound
                    || code == 422 /*validation error*/
                    || code == (int)HttpStatusCode.Conflict
                    || code == (int)HttpStatusCode.BadRequest
                    || code == (int)HttpStatusCode.Forbidden
                    || code == (int)HttpStatusCode.Unauthorized
                    || (code >= 200 && code < 300))
                {
                    return false;
                }

                return true;
            }

            return true;
        }
    }
}
