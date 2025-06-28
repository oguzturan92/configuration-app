using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;
using StackExchange.Redis;

namespace DynamicConfiguration.Core.Storage
{
    public class RedisStorageProvider : IStorageProvider
    {
        private readonly string _connectionString;
        private readonly Lazy<ConnectionMultiplexer> _redis;

        public RedisStorageProvider(string connectionString)
        {
            _connectionString = connectionString;
            _redis = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(_connectionString));
        }

        public async Task<List<ConfigurationItem>> LoadAsync(string applicationName)
        {
            var db = _redis.Value.GetDatabase();
            var result = new List<ConfigurationItem>();

            var keys = await db.SetMembersAsync($"config:{applicationName}:keys");
            foreach (var key in keys)
            {
                var value = await db.StringGetAsync(key.ToString());
                if (!value.IsNullOrEmpty)
                {
                    var item = JsonSerializer.Deserialize<ConfigurationItem>(value!);
                    if (item?.IsActive == true)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }
    }
}