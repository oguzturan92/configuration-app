using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;
using StackExchange.Redis;

namespace DynamicConfiguration.Core.SeedData
{
    public static class RedisSeeder
    {
        public static async Task SeedAsync(string redisConnectionString)
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);
            var db = redis.GetDatabase();

            if (await db.KeyExistsAsync("config:SERVICE-A:SiteName"))
            {
                Console.WriteLine("Seed daha önce yapılmış.");
                return;
            }

            var items = new List<ConfigurationItem>
            {
                new() { Id = 1, Name = "SiteName", Type = "string", Value = "soty.io", IsActive = true, ApplicationName = "SERVICE-A" },
                new() { Id = 2, Name = "IsBasketEnabled", Type = "bool", Value = "1", IsActive = true, ApplicationName = "SERVICE-A" },
                new() { Id = 3, Name = "MaxItemCount", Type = "int", Value = "50", IsActive = true, ApplicationName = "SERVICE-A" },
                new() { Id = 4, Name = "SomeOtherKey", Type = "string", Value = "hidden", IsActive = false, ApplicationName = "SERVICE-A" },
                new() { Id = 5, Name = "AnotherServiceKey", Type = "string", Value = "notForYou", IsActive = true, ApplicationName = "SERVICE-B" },
            };

            foreach (var item in items)
            {
                var json = JsonSerializer.Serialize(item);
                string redisKey = $"config:{item.ApplicationName}:{item.Name}";
                await db.StringSetAsync(redisKey, json);
                await db.SetAddAsync($"config:{item.ApplicationName}:keys", redisKey);
            }

            Console.WriteLine("Redis seed işlemi tamamlandı.");
        }
    }
}