using ClamNet.Client.Exceptions;

namespace ClamNet.Client.Models
{
    internal class RawScanResult
    {
        public RawScanResult(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new RawScanResultEmptyException();
            }

            this.Value = value;
        }

        public string Value { get; }
    }
}