using Newtonsoft.Json;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Results of a get document print api call
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetDocumentsResult : PrintResult
    {
    }
}
