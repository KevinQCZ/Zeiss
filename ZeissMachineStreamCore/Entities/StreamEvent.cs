
using Newtonsoft.Json;

namespace ZeissMachineStreamCore.Entities
{
    public class StreamEvent
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }

        [JsonProperty("join_ref")]
        public string JoinRef { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }
    }
}
