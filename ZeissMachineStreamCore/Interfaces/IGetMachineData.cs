using ZeissMachineStreamCore.Entities;

namespace ZeissMachineStreamCore.Interfaces
{
    /// <summary>
    /// Interface of Getting the data from WEBSOCKET 
    /// </summary>
    public interface IGetMachineData
    {
        /// <summary>
        /// It returns the set of events fetched using the defined filters
        /// </summary>
        /// <param name="filters">The filters to apply, for more info on filters check GetStreamEventsFilters()</param>
        /// <returns>A task that contains the enumerable list of IWebSocketStream that matches the filters</returns>
        Task<IEnumerable<WebSocketStream>> GetEventsByFiltersAsync(IDictionary<string, string> filters);

        /// <summary>
        /// It return the single event that match the defined id
        /// </summary>
        /// <param name="id">The id of the event we want to retrieve</param>
        /// <returns>A task that contains the IWebSocketStream correspond to the id passed as param</returns>
        Task<WebSocketStream> GetEventByIdAsync(string id);
    }
}
