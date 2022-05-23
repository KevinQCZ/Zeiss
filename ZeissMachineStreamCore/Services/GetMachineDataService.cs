using MongoDB.Driver;
using ZeissMachineStreamCore.Entities;
using ZeissMachineStreamCore.Interfaces;

namespace ZeissMachineStreamCore.Services
{
    /// <summary>
    /// Implement of Get Machine Data
    /// </summary>
    public class GetMachineDataService : IGetMachineData
    {
        private readonly IMongo mongo;

        public GetMachineDataService(IMongo mongo)
        {
            this.mongo = mongo;
        }

        public Task<WebSocketStream> GetEventByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WebSocketStream>> GetEventsByFiltersAsync(IDictionary<string, string> filters)
        {
            var (mongoDBFilters, limit) = ParseFilters(filters);
            return await mongo.FindEventsByFiltersAsync(mongoDBFilters, limit: limit);
        }

        private (FilterDefinition<WebSocketStream> mongoDBfilters, int limit) ParseFilters(IDictionary<string, string> filters)
        {
            FilterDefinition<WebSocketStream> mongoDBFilters = Builders<WebSocketStream>.Filter.Eq(x => x.IsDeleted, false);
            int limit = 100;

            foreach (var filter in filters)
                switch (filter.Key)
                {
                    case "machine_id":
                        mongoDBFilters = mongoDBFilters & Builders<WebSocketStream>.Filter.Eq(x => x.StreamEvent.Payload.MachineId, filter.Value);
                        break;
                    case "status":
                        mongoDBFilters = mongoDBFilters & Builders<WebSocketStream>.Filter.In(x => x.StreamEvent.Payload.Status, filter.Value.Split(","));
                        break;
                    case "from":
                        mongoDBFilters = mongoDBFilters & Builders<WebSocketStream>.Filter.Gte(x => x.StreamEvent.Payload.Timestamp, filter.Value);
                        break;
                    case "to":
                        mongoDBFilters = mongoDBFilters & Builders<WebSocketStream>.Filter.Lte(x => x.StreamEvent.Payload.Timestamp, filter.Value);
                        break;
                    case "limit":
                        limit = int.Parse(filter.Value);
                        break;
                    default:
                        break;
                }

            return (mongoDBFilters, limit);
        }
    }
}
