using System.Text;
using System.Threading.Tasks;
using ClamNet.Client.Enums;
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

        [Fact]
        public async Task Ping_ServerOnline_ReturnsExpectedResult()
        {
            // Arrange
            var sut = this.GetSubjectUnderTest();

            // Act
            var response = await sut.Ping();

            // Assert
            Assert.True(response);
        }

        [Theory]
        [InlineData("some clean data")]
        [InlineData("another clean data")]
        public async Task ScanBytes_CleanData_ReturnsExpectedResult(string data)
        {
            // Arrange
            var sut = this.GetSubjectUnderTest();
            var bytes = Encoding.UTF8.GetBytes(data);

            // Act
            var response = await sut.ScanBytes(bytes);

            // Assert
            Assert.Equal(ScanStatus.Clean, response.Status);
            Assert.Empty(response.InfectedFiles);
        }

        [Fact]
        public async Task ScanBytes_InfectedData_ReturnsExpectedResult()
        {
            // Arrange
            const string sampleVirusSignature = @"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*";
            var sut = this.GetSubjectUnderTest();
            var bytes = Encoding.UTF8.GetBytes(sampleVirusSignature);

            // Act
            var response = await sut.ScanBytes(bytes);

            // Assert
            Assert.Equal(ScanStatus.VirusDetected, response.Status);
            Assert.NotEmpty(response.InfectedFiles);
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