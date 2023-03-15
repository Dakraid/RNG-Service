// RNG.Service
// RNG.Service / DataController.cs BY Kristian Schlikow
// First modified on 2023.03.12
// Last modified on 2023.03.13

namespace RNG.Service.Controllers
{
#region
    using Database;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OData.Formatter;
    using Microsoft.AspNetCore.OData.Query;
    using Microsoft.AspNetCore.OData.Results;

    using Models;
#endregion

    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly ILogger<DataController> _logger;

        public DataController(DatabaseContext context, ILogger<DataController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [EnableQuery(PageSize = 15)]
        [HttpGet("Get")]
        public IQueryable<RngEntry> Get() => _context.RngResults.OrderByDescending(p => p.Timestamp).AsQueryable();

        [EnableQuery(PageSize = 15)]
        [HttpGet("GetAllRngAsync")]
        public IQueryable<RngEntry> GetAllRngAsync() => _context.RngResults.OrderByDescending(p => p.Timestamp).AsQueryable();

        [EnableQuery(PageSize = 15)]
        [HttpGet("GetAllTestsAsync")]
        public IQueryable<BatchedTest> GetAllTestsAsync() => _context.TestResults.OrderByDescending(p => p.Timestamp).AsQueryable();

        [EnableQuery]
        [HttpGet("GetRngFromDay/{timestamp:datetime}")]
        public IQueryable<RngEntry> GetRngFromDay([FromODataUri] DateTime timestamp) => _context.RngResults.Where(r => r.Timestamp.Date == timestamp.Date);

        [EnableQuery]
        [HttpGet("GetSingleTest/{key:int}")]
        public SingleResult<BatchedTest> GetSingleTest([FromODataUri] int key)
        {
            var result = _context.TestResults.Where(b => b.ID == key);

            return SingleResult.Create(result);
        }
    }
}
