using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ZeissMachineStreamCore.Config;
using ZeissMachineStreamCore.Entities;
using ZeissMachineStreamCore.Interfaces;

namespace ZeissMachineStreamCore.Services
{
    public class MongoService : IMongo
    {
        private readonly ILogger logger;
        private readonly IMongoClient mongoClient;
        private readonly MongoDBConfiguration mongoDBConfiguration;

        public MongoService(ILogger<MongoService> logger, IMongoClient mongoClient, IOptions<MongoDBConfiguration> optionsMongoDB)
        {
            this.logger = logger;
            mongoDBConfiguration = optionsMongoDB.Value;
            this.mongoClient = mongoClient;
        }

        public async Task InsertEventAsync(WebSocketStream webSocketEvent)
        {
            try
            {
                await GetEventsCollection().InsertOneAsync(webSocketEvent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"InsertEventAsync({webSocketEvent}) using MongoService caused error");
            }
        }

        public async Task<List<WebSocketStream>> FindEventsByFiltersAsync(FilterDefinition<WebSocketStream> filters = null, SortDefinition<WebSocketStream> sort = null, int limit = 100)
        {
            try
            {
                if (filters == null)
                    filters = Builders<WebSocketStream>.Filter.Empty;
                if (sort == null)
                    return await GetEventsCollection().Find(filters).Limit(limit).ToListAsync();
                return await GetEventsCollection().Find(filters).Sort(sort).Limit(limit).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IMongoCollection<WebSocketStream> GetEventsCollection()
        {
            return GetMongoDatabase(mongoDBConfiguration.DatabaseName)
                .GetCollection<WebSocketStream>(mongoDBConfiguration.EventCollection);
        }

        private IMongoDatabase GetMongoDatabase(string databaseName)
        {
            return mongoClient.GetDatabase(databaseName);
        }
    }
}
