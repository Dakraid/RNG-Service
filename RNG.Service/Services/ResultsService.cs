#region header
// RNG.Service
// RNG.Service / ResultsService.cs BY Kristian Schlikow
// First modified on 2023.02.21
// Last modified on 2023.02.27
#endregion

#region
#endregion

#region
#endregion

namespace RNG.Service.Services
{
#region usings
    using Controllers;

    using Database;

    using Models;
#endregion

    public class ResultsService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<RngController> _logger;

        public ResultsService(ILogger<RngController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Task<IQueryable<RngEntry>> GetAllRngAsync() => Task.FromResult(_context.RngResults.OrderByDescending(p => p.Timestamp).AsQueryable());
        public Task<IQueryable<RngEntry>> GetAllFilteredRngAsync(string filter) => Task.FromResult(_context.RngResults.Where(p => p.Requestor != filter).OrderByDescending(p => p.Timestamp).AsQueryable());
        public Task<IQueryable<BatchedTest>> GetAllTestsAsync() => Task.FromResult(_context.TestResults.OrderByDescending(p => p.Timestamp).AsQueryable());
    }
}
