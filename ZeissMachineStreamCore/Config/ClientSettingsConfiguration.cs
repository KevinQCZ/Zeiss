
namespace ZeissMachineStreamCore.Config
{
    public class ClientSettingsConfiguration
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string AuthenticationDB { get; set; }
        public string Mechanism { get; set; }
    }
}
