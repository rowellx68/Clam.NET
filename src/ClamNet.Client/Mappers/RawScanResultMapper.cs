using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClamNet.Client.Enums;
using ClamNet.Client.Exceptions;
using ClamNet.Client.Models;

namespace ClamNet.Client.Mappers
{
    internal static class RawScanResultMapper
    {
        public static ScanResult ToScanResult(this RawScanResult rawScanResult)
        {
            if (rawScanResult == null)
            {
                throw new RawScanResultNullException();
            }

            var rawResult = rawScanResult.Value;

            var infectedFiles = new List<InfectedFile>();
            var status = ScanStatus.Unknown;

            if (rawResult.EndsWith("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                status = ScanStatus.Clean;
            }
            else if (rawResult.EndsWith("ERROR", StringComparison.InvariantCultureIgnoreCase))
            {
                status = ScanStatus.Error;
            }
            else if (rawResult.EndsWith("FOUND", StringComparison.InvariantCultureIgnoreCase))
            {
                status = ScanStatus.VirusDetected;

                var files = rawResult.Split(new[] { "FOUND" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var file in files)
                {
                    var split = file.Trim().Split(' ');

                    var fileName = $"{split[0]?.Replace(":", "")}";
                    var virusName = split[1];

                    infectedFiles.Add(new InfectedFile(fileName, virusName));
                }
            }

            return new ScanResult(status, new ReadOnlyCollection<InfectedFile>(infectedFiles), rawResult);
        }
    }
}