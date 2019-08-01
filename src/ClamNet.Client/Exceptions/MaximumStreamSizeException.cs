using System;

namespace ClamNet.Client.Exceptions
{
    public class MaximumStreamSizeException : Exception
    {
        public MaximumStreamSizeException(long maxStreamSize)
            : base($"Maximum stream size of {maxStreamSize} bytes has been exceeded.")
        {
        }
    }
}