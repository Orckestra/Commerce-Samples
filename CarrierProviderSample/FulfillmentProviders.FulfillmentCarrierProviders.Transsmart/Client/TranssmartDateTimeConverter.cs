using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Transsmart.Client
{
    /// <summary>
    /// Transsmart dates are stored in UTC+1 timezone
    /// </summary>
    public class TranssmartDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Converter will set the format for date and time properties
        /// </summary>
        public TranssmartDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// Take a datetime and convert from UTC to UTC+1 for output
        /// </summary>
        /// <param name="writer">json writer</param>
        /// <param name="value">value to convert</param>
        /// <param name="serializer">json serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            DateTime date = ((DateTime)value).AddHours(1);
            base.WriteJson(writer, date, serializer);
        }

        /// <summary>
        /// Take in a string date/time and convert from UTC+1 to UTC
        /// </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">type of object</param>
        /// <param name="existingValue">existing value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns>the converted object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // no need to check for null, as it will only be here if it has a date value
            var transsmartDate = reader.Value is DateTime ? (DateTime)reader.Value :
                DateTime.ParseExact((string)reader.Value, DateTimeFormat, CultureInfo.InvariantCulture);
            var date = DateTime.SpecifyKind(transsmartDate.AddHours(-1), DateTimeKind.Utc);
            return date;
        }
    }
}
