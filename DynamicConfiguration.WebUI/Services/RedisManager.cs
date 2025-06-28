using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DynamicConfiguration.WebUI.Services
{
    public class RedisManager : IRedisService
    {
        private readonly IDatabase _db;

        public RedisManager()
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { "redis-12103.c328.europe-west3-1.gce.redns.redis-cloud.com:12103" },
                User = "default",
                Password = "C4al4211Dz8HlIpDCox5PR6CJuHbr41J",
                Ssl = false,
                AbortOnConnectFail = false
            };

            var redis = ConnectionMultiplexer.Connect(config);
            _db = redis.GetDatabase();
        }

        public async Task SaveAsync(ConfigurationItem item)
        {
            var json = JsonSerializer.Serialize(item);
            var key = $"config:{item.ApplicationName}:{item.Name}";
            await _db.StringSetAsync(key, json);
            await _db.SetAddAsync($"config:{item.ApplicationName}:keys", key);
        }

        public async Task DeleteAsync(string applicationName, string name)
        {
            var key = $"config:{applicationName}:{name}";
            await _db.KeyDeleteAsync(key); // redis'ten sileriz
            await _db.SetRemoveAsync($"config:{applicationName}:keys", key); // keys set'inden çıkarırız
        }
    }
}