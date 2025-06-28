using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;

namespace DynamicConfiguration.Core.Storage
{
    public interface IStorageProvider
    {
        Task<List<ConfigurationItem>> LoadAsync(string applicationName);
    }
}