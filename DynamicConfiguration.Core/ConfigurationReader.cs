using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DynamicConfiguration.Core.Cache;
using DynamicConfiguration.Core.Helpers;
using DynamicConfiguration.Core.Storage;


namespace DynamicConfiguration.Core
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly IStorageProvider _storageProvider;
        private readonly ConfigurationCache _cache = new();
        private readonly System.Timers.Timer _refreshTimer;

        public ConfigurationReader(string applicationName, string connectionString, double refreshTimerIntervalInMs)
        {
            _applicationName = applicationName;

            // MSSQL PROVIDER
            // _storageProvider = new SqlStorageProvider(connectionString);

            // REDIS PROVIDER
            _storageProvider = new RedisStorageProvider(connectionString);


            LoadInitialConfiguration().GetAwaiter().GetResult(); // İlk yükleme

            _refreshTimer = new System.Timers.Timer(refreshTimerIntervalInMs);
            // _refreshTimer.Elapsed += async (_, _) => await RefreshAsync();
            _refreshTimer.Elapsed += RefreshTimerElapsed;
            _refreshTimer.Start();
        }

        //  Olası concurrency problemlerini engelleyecek yapı (RabbitMq için işe yaramaz)
        private async void RefreshTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _refreshTimer.Stop(); // re-entrancy'yi engelle
            try
            {
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Timer refresh error: " + ex.Message);
            }
            finally
            {
                _refreshTimer.Start();
            }
        }

        public T GetValue<T>(string key)
        {
            Console.WriteLine("GetValue çalıştı. İşlem zamanı: " + DateTime.Now);
            var item = _cache.Get(key);
            if (item is null)
            {
                Console.WriteLine($"Anahtar bulunamadı: {key}");
                return default!;
            }

            Console.WriteLine($"Anahtar bulundu: {key} = {item.Value}");
            return ValueConverter.Convert<T>(item.Value);
        }

        private async Task LoadInitialConfiguration()
        {
            try
            {
                var items = await _storageProvider.LoadAsync(_applicationName);
                _cache.Update(items);
                Console.WriteLine("LoadInitialConfiguration başarılı. İşlem zamanı: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İlk yükleme başarısız oldu. Message: {ex.Message}");
            }
        }

        private async Task RefreshAsync()
        {
            try
            {
                var items = await _storageProvider.LoadAsync(_applicationName);
                _cache.Update(items);
                Console.WriteLine("Refresh başarılı. İşlem zamanı: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh sırasında storage erişilemedi. Message: {ex.Message}");
            }
        }
    }
}