using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeissMachineStreamCore.Entities
{
    /// <summary>
    /// The Specified filters
    /// </summary>
    public class SpecificationFilters
    {
        /// <summary>
        /// The name of the filter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the filters
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the filter
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public string Format { get; set; }

        public SpecificationFilters(string name, string type, string description, string? format = null, IEnumerable<string>? values = null)
        {
            Name = name;
            Type = type;
            Description = description;
            Format = format;
        }
    }
}
