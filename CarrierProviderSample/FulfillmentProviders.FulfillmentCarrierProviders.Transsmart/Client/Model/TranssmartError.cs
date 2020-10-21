using Newtonsoft.Json;
using System;
using System.Text;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Result obtained from a bad request or errors
    /// </summary>
    public class TranssmartError
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Output error in human readable format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            if (TimeStamp != DateTime.MinValue)
            {
                result.Append(TimeStamp.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                result.Append($" Code: {Code}");
            }

            if (!string.IsNullOrWhiteSpace(Message))
            {
                result.Append($" Message: {Message}");
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                result.Append($" Description: {Description}");
            }

            if (!string.IsNullOrWhiteSpace(Status))
            {
                result.Append($" Status: {Status}");
            }
            return result.ToString();
        }
    }
}
