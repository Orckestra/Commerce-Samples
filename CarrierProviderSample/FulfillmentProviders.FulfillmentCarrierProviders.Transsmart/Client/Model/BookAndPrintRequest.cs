using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Book and Print api command
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BookAndPrintRequest : BookRequest
    {
    }
}
