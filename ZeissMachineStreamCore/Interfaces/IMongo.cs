using MongoDB.Driver;
using ZeissMachineStreamCore.Entities;

namespace ZeissMachineStreamCore.Interfaces
{
    /// <summary>
    /// Interface of Manage MongoDB
    /// </summary>
    public interface IMongo
    {
        Task InsertEventAsync(WebSocketStream WebSocketStream);

        Task<List<WebSocketStream>> FindEventsByFiltersAsync(FilterDefinition<WebSocketStream> filters, SortDefinition<WebSocketStream> sort = null, int limit = 100);
    }
}
