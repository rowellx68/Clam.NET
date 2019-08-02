using System.Threading.Tasks;
using Xunit;

namespace ClamNet.Client.IntegrationTests
{
    public class ScanClientTests
    {
        [Fact]
        public async Task GetVersion_ReturnsExpectedResult()
        {
            // Arrange
            var sut = this.GetSubjectUnderTest();

            // Act
            var response = await sut.GetVersion();

            // Assert
            Assert.StartsWith("ClamAV", response);
        }

        #region Setup

        private IScanClient GetSubjectUnderTest()
        {
            var executor = new CommandExecutor(new SocketClient(), "localhost", 3310);
            return new ScanClient(executor);
        }

        #endregion
    }
}