using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;

namespace DynamicConfiguration.WebUI.Services
{
    public interface IRedisService
    {
        Task SaveAsync(ConfigurationItem item);
        Task DeleteAsync(string applicationName, string name);
    }
}