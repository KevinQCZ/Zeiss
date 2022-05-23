using MongoDB.Driver;
using System.Security.Authentication;
using ZeissMachineStreamCore.Config;

namespace ZeissMachineStreamCore.Services
{
    public static class MongoSettings
    {
        public static IMongoClient GetMongoClient(ClientSettingsConfiguration clientSettingsConfiguration)
        {
            return new MongoClient(GetMongoClientSettings(clientSettingsConfiguration));
        }

        private static MongoClientSettings GetMongoClientSettings(ClientSettingsConfiguration clientSettingsConfiguration)
        {
            MongoClientSettings settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(clientSettingsConfiguration.Server, clientSettingsConfiguration.Port)
            };

            if (clientSettingsConfiguration.UseSSL)
            {
                settings.UseSsl = true;
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            }

            MongoIdentity identity = new MongoInternalIdentity(clientSettingsConfiguration.AuthenticationDB, clientSettingsConfiguration.UserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(clientSettingsConfiguration.Password);

            settings.Credential = new MongoCredential(clientSettingsConfiguration.Mechanism, identity, evidence);

            return settings;
        }
    }
}
