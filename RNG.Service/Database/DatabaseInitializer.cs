#region header
// RNG.Service
// RNG.Service / DatabaseInitializer.cs BY Kristian Schlikow
// First modified on 2023.02.23
// Last modified on 2023.02.27
#endregion

namespace RNG.Service.Database
{
    public class DatabaseInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
