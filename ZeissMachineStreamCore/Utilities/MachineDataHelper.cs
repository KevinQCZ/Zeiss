using ZeissMachineStreamCore.Entities;

namespace ZeissMachineStreamCore.Utilities
{
    public static class MachineDataHelper
    {
        public static readonly string[] MachineStreamStatus = { "idle", "running", "finished", "errorred", "repaired" };

        /// <summary>
        /// Get all the possible application filters to the events
        /// </summary>
        /// <returns>List of all specification filters</returns>
        public static IEnumerable<SpecificationFilters> GetMachineStreamEventsFilters()
        {
            List<SpecificationFilters> specificationFilters = new List<SpecificationFilters>();

            specificationFilters.Add(new SpecificationFilters("machine_id", "guid", "the machine id of the event"));
            specificationFilters.Add(new SpecificationFilters("id", "guid", "the id of the event"));
            specificationFilters.Add(new SpecificationFilters("status", "string", "A comma separated string of the possible values of the status event flag", format: null, MachineStreamStatus));
            specificationFilters.Add(new SpecificationFilters("timestamp", "date", "the date of the reported timestamp (in string format)", format: "YYYY-MM-DDTHH:mm.ssssssZ"));

            return specificationFilters;
        }

        /// <summary>
        /// Validate filter in the request
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>Return validation errors</returns>
        public static IEnumerable<string> ValidateFilters(IDictionary<string, string> filters)
        {
            List<string> filterErrors = new List<string>();
            var availableFilters = GetMachineStreamEventsFilters();

            if (filters.Keys.All(x => availableFilters.Select(y => y.Name).Contains(x)))
            {
                //check machine_id
                Guid guid = Guid.Empty;
                if (filters.Keys.Contains("machine_id") && !Guid.TryParse(filters["machine_id"], out guid))
                    filterErrors.Add("machine_id is not a valid guid");
                //check id
                Guid id = Guid.Empty;
                if (filters.Keys.Contains("id") && !Guid.TryParse(filters["id"], out id))
                    filterErrors.Add("id is not a valid guid");
                //check timestamp
                DateTime dateTime = DateTime.UtcNow;
                if (filters.Keys.Contains("timestamp") && filters["timestamp"].Length == 27 && !DateTime.TryParse(filters["timestamp"], out dateTime))
                    filterErrors.Add("timestamp is not a valid datetime");
                //check status
                if (filters.Keys.Contains("status") && !filters["status"].Split(",").All(x => MachineStreamStatus.Contains(x)))
                    filterErrors.Add("one or more statuses are not valid, please check");
            }
            else
            {
                filterErrors.Add("Filters validate failed, please check the request.");
            }
            return filterErrors;
        }
    }
}
