using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace Transsmart.Client.Model
{
    /// <summary>
    /// Dangerous goods information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DangerousGoodsInformation
    {
        /// <summary>
        /// Gets or sets dangerous goods lines
        /// </summary>
        [JsonProperty(PropertyName = "dangerousGoods")]
        [StringLength(64)]
        public DangerousGoods[] DangerousGoods { get; set; }
    }
}
