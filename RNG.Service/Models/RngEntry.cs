#region header
// RNG.Service
// RNG.Service / RngEntry.cs BY Kristian Schlikow
// First modified on 2023.02.23
// Last modified on 2023.02.27
#endregion

namespace RNG.Service.Models
{
    public class RngEntry
    {
        // ReSharper disable once InconsistentNaming
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public double Result { get; set; }
        public string Requestor { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public int BatchedTestID { get; set; }
    }
}
