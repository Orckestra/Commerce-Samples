using System;

namespace Transsmart.Client.Model
{

    /// <summary>
    /// Transsmart exception (for errors from API calls)
    /// </summary>
    public class TranssmartException : Exception
    {
        public TranssmartException(string message, Exception inner) : base(message, inner)
        { }
    }
}
