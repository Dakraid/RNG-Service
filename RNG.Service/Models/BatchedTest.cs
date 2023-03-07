#region header
// RNG.Service
// RNG.Service / BatchedTest.cs BY Kristian Schlikow
// First modified on 2023.02.21
// Last modified on 2023.02.27
#endregion

namespace RNG.Service.Models
{
    public class BatchedTest
    {
        // ReSharper disable once InconsistentNaming
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }

        public ICollection<RngEntry> RngList { get; set; } = new List<RngEntry>();
    }
}
