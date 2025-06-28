using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicConfiguration.Core.Helpers
{
    public static class ValueConverter
    {
        // TASK : Kütüphane her tipe ait dönüş bilgisini kendi içerisinde halletmelidir
        public static T Convert<T>(string raw)
        {
            try
            {
                var targetType = typeof(T);

                if (targetType == typeof(bool))
                {
                    if (raw == "1" || raw.Equals("true", StringComparison.OrdinalIgnoreCase))
                        return (T)(object)true;
                    if (raw == "0" || raw.Equals("false", StringComparison.OrdinalIgnoreCase))
                        return (T)(object)false;

                    throw new InvalidCastException($"Bool dönüşümünde geçersiz değer: {raw}");
                }

                return (T)System.Convert.ChangeType(raw, targetType)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dönüşüm hatası var: {raw} => {typeof(T).Name}. Message: {ex.Message}");
                return default!;
            }
        }
    }
}