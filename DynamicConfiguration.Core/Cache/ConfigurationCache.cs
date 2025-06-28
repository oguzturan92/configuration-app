using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;

namespace DynamicConfiguration.Core.Cache
{
    public class ConfigurationCache
    {
        private readonly ConcurrentDictionary<string, ConfigurationItem> _cache = new();

        public void Update(List<ConfigurationItem> items)
        {
            _cache.Clear();
            foreach (var item in items)
            {
                _cache[item.Name] = item;
            }
        }

        public ConfigurationItem? Get(string key)
        {
            _cache.TryGetValue(key, out var item);
            return item;
        }

        public IReadOnlyDictionary<string, ConfigurationItem> GetAll()
        {
            return _cache;
        }
    }
}