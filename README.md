
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


![Image](https://github.com/user-attachments/assets/45989fd5-505f-45a9-8fed-21707e620987)


![Image](https://github.com/user-attachments/assets/1e0acc01-d891-434d-b6ef-53b138d6e016)
