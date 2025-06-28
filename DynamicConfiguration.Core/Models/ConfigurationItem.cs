using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicConfiguration.Core.Models
{
    public class ConfigurationItem
    {
        public int Id { get; set; } // 1, 2, 3
        public string Name { get; set; } = null!; // SiteName, IsBasketEnabled, MaxItemCount
        public string Type { get; set; } = null!; // string, bool, int
        public string Value { get; set; } = null!; // soty.io, 1, 50
        public bool IsActive { get; set; } // 1, 1, 0
        public string ApplicationName { get; set; } = null!; // SERVICE-A, SERVICE-B
    }
}