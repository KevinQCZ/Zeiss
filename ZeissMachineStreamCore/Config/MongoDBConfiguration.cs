
namespace ZeissMachineStreamCore.Config
{
    public class MongoDBConfiguration
    {
        public ClientSettingsConfiguration ClientSettings { get; set; }
        public string DatabaseName { get; set; }
        public string EventCollection { get; set; }
    }
}
