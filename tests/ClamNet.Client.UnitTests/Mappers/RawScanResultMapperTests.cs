using System.Linq;
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

        [Theory]
        [InlineData("error")]
        [InlineData("ERROR")]
        public void ToScanResult_ScanError_ReturnsExpectedResult(string value)
        {
            // Arrange
            var rawResult = new RawScanResult(value);

            // Act
            var result = rawResult.ToScanResult();

            // Assert
            Assert.Equal(ScanStatus.Error, result.Status);
            Assert.Empty(result.InfectedFiles);
        }

        [Theory]
        [InlineData("stream: Some-Virus-Name found")]
        [InlineData("stream: Some-Virus-Name FOUND")]
        public void ToScanResult_VirusFound_ReturnsExpectedResult(string value)
        {
            // Arrange
            var rawResult = new RawScanResult(value);

            // Act
            var result = rawResult.ToScanResult();

            // Assert
            Assert.Equal(ScanStatus.VirusDetected, result.Status);
            Assert.Single(result.InfectedFiles);

            var infected = result.InfectedFiles.First();
            Assert.Equal(value, result.RawResult);
            Assert.Equal("stream", infected.FileName);
            Assert.Equal("Some-Virus-Name", infected.VirusName);
        }
    }
}