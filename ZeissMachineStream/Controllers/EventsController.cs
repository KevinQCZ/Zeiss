using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeissMachineStreamCore.Entities;
using ZeissMachineStreamCore.Interfaces;
using ZeissMachineStreamCore.Utilities;
using static ZeissMachineStream.Models.ResponseModel;

namespace ZeissMachineStream.Controllers
{
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly ILogger logger;
        private readonly IGetMachineData getMachineData;

        public EventsController(IGetMachineData getMachineData, ILogger<EventsController> logger)
        {
            this.logger = logger;
            this.getMachineData = getMachineData;
        }

        /// <summary>
        /// Get the events applying the filters passed in query string (Optional)
        /// </summary>
        /// <param name="filters">Optional. The available filter to apply. Check GetFilterSpecs for more info.</param>
        /// <returns>The list of the events with the applied filters</returns>
        /// <response code="200">The list is retrieved correctly applying the filters</response>
        /// <response code="400">The filters passed in query string are not formatted correctly</response>
        /// <response code="404">No events available for the provided filters</response>
        /// <response code="500">Server error or the service is currently unavailable</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StreamEvent>), 200)]
        [ProducesResponseType(typeof(ApiBadRequestModel), 400)]
        [ProducesResponseType(typeof(ApiNotFoundModel), 404)]
        [ProducesResponseType(typeof(ApiServerErrorModel), 500)]
        public async Task<ActionResult<IEnumerable<StreamEvent>>> GetByFilters([FromQuery] Dictionary<string, string> filters)
        {
            var validation = MachineDataHelper.ValidateFilters(filters);
            if (validation.Count() > 0)
                return BadRequest(new ApiBadRequestModel
                {
                    Params = filters,
                    BadRequestReasons = validation
                });
            else
            {
                try
                {
                    var webSocketStreams = await getMachineData.GetEventsByFiltersAsync(filters);
                    if (webSocketStreams.Count() > 0)
                        return Ok(webSocketStreams.Select(x => x.StreamEvent));

                    return NotFound(new ApiNotFoundModel
                    {
                        Params = filters,
                        NotFoundReasons = new string[] { "Not found any events by the filters" }
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, message: $"EventsController.GetByFilters({filters}) caused an error");
                    return StatusCode(500, new ApiServerErrorModel
                    {
                        Params = filters,
                        ServerErrors = new string[] { $"Get data by filter failed or the service is currently unavailable, details please check the log" }
                    });
                }
            }
        }

        /// <summary>
        /// Fetch the event corresponding to the id passed as URL parameter
        /// </summary>
        /// <param name="id">The id of the event</param>
        /// <returns></returns>
        /// <response code="200">The event corresponding to the id passed as URL parameter</response>
        /// <response code="404">The event for the specified id is not found or is been deleted</response>
        /// <response code="500">Server error or the service is currently unavailable</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StreamEvent), 200)]
        [ProducesResponseType(typeof(ApiNotFoundModel), 404)]
        [ProducesResponseType(typeof(ApiServerErrorModel), 500)]
        public async Task<ActionResult<StreamEvent>> GetById(string id)
        {
            try
            {
                var webSocketStream = await getMachineData.GetEventByIdAsync(id);
                if (webSocketStream != null)
                    return Ok(webSocketStream.StreamEvent);
                else
                    return NotFound(new ApiNotFoundModel
                    {
                        Params = id,
                        NotFoundReasons = new string[] { $"Not found any events by the filters" }
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"EventsController.GetById({id}) caused an error");
                return StatusCode(500, new ApiServerErrorModel
                {
                    Params = id,
                    ServerErrors = new string[] { $"Get data by filter failed or the service is currently unavailable, details please check the log" }
                });
            }
        }

    }
}
