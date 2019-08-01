using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClamNet.Client.Enums;
using NSubstitute;
using Xunit;

namespace ClamNet.Client.UnitTests
{
    public class ScanClientTests
    {
        [Fact]
        public async Task GetVersion_ReturnsExpectedResult()
        {
            // Arrange
            const string version = "ClamAV 0.101.2/25528/Thu Aug  1 09:29:44 2019";
            const string expected = "ClamAV 0.101.2/25528/Thu Aug  1 09:29:44 2019";

            var sut = this.GetSubjectUnderTest();
            this.CommandExecutor
                .Execute(Arg.Is(ScanClientCommand.Version))
                .Returns(version);

            // Act
            var response = await sut.GetVersion();

            // Assert
            Assert.Equal(expected, response);
        }

        [Theory]
        [InlineData("pong", true)]
        [InlineData("PONG", true)]
        [InlineData("ok", false)]
        public async Task Ping_ReturnsExpectedResult(string pong, bool expected)
        {
            // Arrange
            var sut = this.GetSubjectUnderTest();
            this.CommandExecutor
                .Execute(Arg.Is(ScanClientCommand.Ping))
                .Returns(pong);

            // Act
            var response = await sut.Ping();

            // Assert
            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task ScanBytes_BytesClean_ReturnsExpectedResult()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("Clean content");

            const string rawResult = "stream: OK";

            var sut = this.GetSubjectUnderTest();
            this.CommandExecutor
                .Execute(Arg.Is(ScanClientCommand.InStream), Arg.Any<Func<NetworkStream, Task>>())
                .Returns(rawResult);

            // Act
            var response = await sut.ScanBytes(bytes);

            // Assert
            Assert.Equal(ScanStatus.Clean, response.Status);
            Assert.Empty(response.InfectedFiles);
            Assert.Equal(rawResult, response.RawResult);
            Assert.Equal(rawResult, response.ToString());
        }

        [Fact]
        public async Task ScanBytes_BytesVirus_ReturnsExpectedResult()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("Virus content");

            const string rawResult = "stream: Eicar-Test-Signature FOUND";

            var sut = this.GetSubjectUnderTest();
            this.CommandExecutor
                .Execute(Arg.Is(ScanClientCommand.InStream), Arg.Any<Func<NetworkStream, Task>>())
                .Returns(rawResult);

            // Act
            var response = await sut.ScanBytes(bytes);

            // Assert
            Assert.Equal(ScanStatus.VirusDetected, response.Status);
            Assert.Single(response.InfectedFiles);
            Assert.Equal(rawResult, response.RawResult);
            Assert.Equal(rawResult, response.ToString());

            var infectedInfo = response.InfectedFiles.First();

            Assert.Equal("stream", infectedInfo.FileName);
            Assert.Equal("Eicar-Test-Signature", infectedInfo.VirusName);
        }

        #region Setup

        private ICommandExecutor CommandExecutor { get; } = Substitute.For<ICommandExecutor>();

        private IScanClient GetSubjectUnderTest()
        {
            return new ScanClient(this.CommandExecutor);
        }

        #endregion
    }
}