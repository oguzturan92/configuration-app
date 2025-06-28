
# HAKKINDA
- Bu uygulama, dinamik bir konfigürasyon yapısı uygulamasıdır.
- Appkey'ler, ortak ve dinamik bir yapıyla erişilebilir.
- Deployment veta restart gerektirmez.
- Konfigürasyon kayıtları farklı tiplerde bile olsa, kendi tipinde döner.
- Sadece true olan sonuçlar döner.
- Storage olarak mssql ve redis kullanıldı (Ana kayıtlar mssql'de tutuluyor ve her crud işleminde mssql ve redis'teki bilgiler de güncelleniyor. Konfigürasyon bilgileri ise sadece redis'ten alınıyor.)
- .net 8 kullanıldı
- Kütüphane, redis'e erişemediğinde, başarılı son konfigürasyon kayıtları ile devam ediyor. Erişim sağlandığında ise, güncel verilerle devam ediyor.
- Güncellemeler belirli aralıklarla kontrol ediliyor.
- Her servis, sadece kendi konfigürasyon kayıtlarına erişebiliyor.
- async / await kullanıldı.
- Olası concurrency problemleri engellendi.
- Unit test örneği yazıldı
- Design ve Architectural Pattern'ler kullanıldı.
- TDD (Test Driven Development) yazıldı

# KURULUM

# mssql'de bir tablo oluşturun
CREATE DATABASE dbName;

# veritabanınıza konumlanın
USE dbName;

# tablo ekleyin
CREATE TABLE ConfigurationItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(20) NOT NULL, -- string, bool, int, double
    Value NVARCHAR(500) NOT NULL,
    IsActive BIT NOT NULL,
    ApplicationName NVARCHAR(100) NOT NULL
);

# bilgileri ekleyin
INSERT INTO ConfigurationItems (Name, Type, Value, IsActive, ApplicationName)
VALUES 
('SiteName', 'string', 'soty.io', 1, 'SERVICE-A'),
('IsBasketEnabled', 'bool', 'true', 1, 'SERVICE-A'),
('MaxItemCount', 'int', '50', 1, 'SERVICE-A'),
('SomeOtherKey', 'string', 'hidden', 0, 'SERVICE-A'),
('AnotherServiceKey', 'string', 'notForYou', 1, 'SERVICE-B');

# webuı katmanını çalıştırın ve /configuration adresine giderek crud işlemlerini gerçekleştirin (dotnet run)
# SampleApp console projesini çalıştırın (dotnet run)
# test için test projesini çalıştırın (dotnet test)


# ÇALIŞMA ANINA AİT GÖRSELLER

# Belirli Aralıklarla Kontrol Edilmesi ve SERVICE-B'nin, SERVICE-A'nın Value'suna Erişemiyor Olması
![Image](https://github.com/user-attachments/assets/45989fd5-505f-45a9-8fed-21707e620987)

# Storage'ye Erişim Olmadığında, Cache'den Verilerin Alınması
![Image](https://github.com/user-attachments/assets/1e0acc01-d891-434d-b6ef-53b138d6e016)
