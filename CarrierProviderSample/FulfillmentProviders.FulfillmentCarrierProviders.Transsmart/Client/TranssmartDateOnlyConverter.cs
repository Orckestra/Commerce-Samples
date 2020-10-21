using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Transsmart.Client
{
    /// <summary>
    /// Transsmart date only fields have a specific format
    /// </summary>
    public class TranssmartDateOnlyConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Converter will set the format for date only properties
        /// </summary>
        public TranssmartDateOnlyConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
