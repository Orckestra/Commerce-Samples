using Newtonsoft.Json;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a Book and Print api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BookAndPrintResult : PrintResult
    {
    }
}
