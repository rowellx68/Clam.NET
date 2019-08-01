using ClamNet.Client.Enums;
using ClamNet.Client.Exceptions;
using ClamNet.Client.Mappers;
using ClamNet.Client.Models;
using Xunit;

namespace ClamNet.Client.UnitTests.Mappers
{
    public class RawScanResultMapperTests
    {
        [Fact]
        public void ToScanResult_NullRawScanResult_ThrowsRawScanResultNullException()
        {
            // Arrange
            var rawResult = null as RawScanResult;

            // Act/Assert
            Assert.Throws<RawScanResultNullException>(() => rawResult.ToScanResult());
        }

        [Theory]
        [InlineData("ok")]
        [InlineData("OK")]
        public void ToScanResult_NoVirusFound_ReturnsExpectedResult(string value)
        {
            // Arrange
            var rawResult = new RawScanResult(value);

            // Act
            var result = rawResult.ToScanResult();

            // Assert
            Assert.Equal(ScanStatus.Clean, result.Status);
            Assert.Empty(result.InfectedFiles);
        }
    }
}