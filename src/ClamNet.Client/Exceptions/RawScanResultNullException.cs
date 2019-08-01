using System;

namespace ClamNet.Client.Exceptions
{
    public class RawScanResultNullException : Exception
    {
        public RawScanResultNullException() : base("Raw scan result cannot be empty.")
        {
        }
    }
}