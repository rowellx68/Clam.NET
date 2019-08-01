using ClamNet.Client.Exceptions;
using ClamNet.Client.Models;
using Xunit;

namespace ClamNet.Client.UnitTests.Models
{
    public class RawScanResultTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_NullEmptyWhitespaceValue_ThrowsRawScanResultEmptyException(string value)
        {
            // Act/Assert
            Assert.Throws<RawScanResultEmptyException>(() => new RawScanResult(value));
        }
    }
}