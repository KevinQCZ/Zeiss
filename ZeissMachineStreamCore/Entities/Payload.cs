using Newtonsoft.Json;

namespace ZeissMachineStreamCore.Entities
{
    public class Payload
    {
        [JsonProperty("machine_id")]
        public string MachineId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
