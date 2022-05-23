using Newtonsoft.Json;
namespace ZeissMachineStream.Models
{
    /// <summary>
    /// The base api response model, it contains only the Params object that
    /// represent the params used in the call
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Optional object to identity the params used in the call
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Params { get; set; }
    }
}
