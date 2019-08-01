using System.Collections.Generic;
using ClamNet.Client.Enums;

namespace ClamNet.Client.Models
{
    public class ScanResult
    {
        public ScanResult(ScanStatus status,
            IReadOnlyCollection<InfectedFile> infectedFiles,
            string rawResult)
        {
            this.Status = status;
            this.InfectedFiles = infectedFiles;
            this.RawResult = rawResult;
        }

        public ScanStatus Status { get; }

        public IReadOnlyCollection<InfectedFile> InfectedFiles { get; }

        public string RawResult { get; }

        public override string ToString()
        {
            return this.RawResult;
        }
    }
}