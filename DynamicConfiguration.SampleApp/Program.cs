﻿using DynamicConfiguration.Core;
using DynamicConfiguration.Core.SeedData;

var delayTime = 3000;

// MSSQL
// var connectionString = "Server=.\\SQLEXPRESS;Database=dbName;Integrated Security=True;TrustServerCertificate=True;";

// REDIS
var connectionString = "redis-12103.c328.europe-west3-1.gce.redns.redis-cloud.com:12103,password=sifreniz,ssl=False,abortConnect=false";

// SEED VERİLERİ - MSSQL AÇIK OLDUĞUNDA BUNU KAPATIN
await RedisSeeder.SeedAsync(connectionString);

var readerServiceA = new ConfigurationReader("SERVICE-A", connectionString, delayTime);
var readerServiceB = new ConfigurationReader("SERVICE-B", connectionString, delayTime);

while (true)
{
    Console.WriteLine("START ================================================================");

    var siteName = readerServiceA.GetValue<string>("SiteName");
    Console.WriteLine($"SERVICE-A - SiteName: {siteName}");

    var isBasketEnabled = readerServiceA.GetValue<bool>("IsBasketEnabled");
    Console.WriteLine($"SERVICE-A - IsBasketEnabled: {isBasketEnabled}");

    var maxItemCount = readerServiceA.GetValue<int>("MaxItemCount");
    Console.WriteLine($"SERVICE-A - MaxItemCount: {maxItemCount}");

    // TASK :  Herservis yalnızca kendi konfigürasyon kayıtlarına erişebilmeli, başkasının kayıtlarını görmemelidir.
    // Burda A seçeneğinde SERVICE-A uygulamasına ait Value bilgisi gelmekte ancak SERVICE-B uygulaması SiteName key'ine ait Value'yu göremiyor. SERVICE-B, kendi Value'suna AnotherServiceKey key'i ile ulaşabilir
    // A)
    var valueServiceB = readerServiceB.GetValue<string>("SiteName");
    Console.WriteLine("SERVICE-B Uygulaması, SERVICE-A'nın key'ini kullanarak SERVICE-A'nın value'suna erişemiyor : " + valueServiceB);

    Console.WriteLine("FINISH ================================================================");
    await Task.Delay(delayTime);
}
