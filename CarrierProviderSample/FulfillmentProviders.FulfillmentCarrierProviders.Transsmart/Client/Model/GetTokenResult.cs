using Newtonsoft.Json;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Returns the Get Token API results
    /// </summary>
    public class GetTokenResult
    {
        /// <summary>
        /// Gets or sets the token used for using the api
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
