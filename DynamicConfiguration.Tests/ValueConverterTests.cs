using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Helpers;
using Xunit;

namespace DynamicConfiguration.Tests
{
    public class ValueConverterTests
    {
        // namespace DynamicConfiguration.Core.Helpers'taki ValueConverter'ın tip dönüştürmesinin test edilmesi.

        [Theory]
        [InlineData("true", true)]
        [InlineData("1", true)]
        [InlineData("false", false)]
        [InlineData("0", false)]
        public void Convert_BoolValues_ReturnsExpectedResult(string input, bool expected)
        {
            var result = ValueConverter.Convert<bool>(input);
            Assert.Equal(expected, result);
        }

        // "123" değerinin int'e dönüşümünü test eder
        [Fact]
        public void Convert_Int_ReturnsExpected()
        {
            var result = ValueConverter.Convert<int>("123");
            Console.WriteLine("Convert_Int_ReturnsExpected : " + result);
            Assert.Equal(123, result);
        }

        // "abc" değeri int'e dönüşemeyeceği için hata verir
        [Fact]
        public void Convert_InvalidInt_ReturnsDefault()
        {
            var result = ValueConverter.Convert<int>("abc");
            Console.WriteLine("Convert_InvalidInt_ReturnsDefault : " + result);
            Assert.Equal(0, result);
        }

        // "soty.io" sonucu düzgün çalışıyor mu diye kontrol eder
        [Fact]
        public void Convert_String_ReturnsExpected()
        {
            var result = ValueConverter.Convert<string>("soty.io");
            Console.WriteLine("Convert_String_ReturnsExpected : " + result);
            Assert.Equal("soty.io", result);
        }
    }
}