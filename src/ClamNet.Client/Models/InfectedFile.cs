namespace ClamNet.Client.Models
{
    public class InfectedFile
    {
        public InfectedFile(string fileName, string virusName)
        {
            this.FileName = fileName;
            this.VirusName = virusName;
        }

        public string FileName { get; }

        public string VirusName { get; }
    }
}