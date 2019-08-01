using System;

namespace ClamNet.Client.Exceptions
{
    public class RawScanResultEmptyException : Exception
    {
        public RawScanResultEmptyException() : base("Raw scan result cannot be empty.")
        {
        }
    }
}