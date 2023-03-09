// $SOLUTION$
// $PROJECT$ / DataController.cs BY Kristian Schlikow
// First modified on $CREATED_YEAR$.$CREATED_MONTH$.$CREATED_DAY$
// Last modified on $CURRENT_YEAR$.$CURRENT_MONTH$.$CURRENT_DAY$

namespace RNG.Service.Controllers
{
    using Database;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OData.Formatter;
    using Microsoft.AspNetCore.OData.Query;
    using Microsoft.AspNetCore.OData.Results;
    using Microsoft.AspNetCore.OData.Routing.Controllers;

    using Models;

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
        [HttpGet("GetAllRngAsync")]
        public IQueryable<RngEntry> GetAllRngAsync() => _context.RngResults.OrderByDescending(p => p.Timestamp).AsQueryable();

        [EnableQuery(PageSize = 15)]
        [HttpGet("GetAllTestsAsync")]
        public IQueryable<BatchedTest> GetAllTestsAsync() => _context.TestResults.OrderByDescending(p => p.Timestamp).AsQueryable();

        [EnableQuery]
        [HttpGet("GetRngFromDay/{timestamp}")]
        public IQueryable<RngEntry> GetRngFromDay([FromODataUri] DateTime timestamp) => _context.RngResults.Where(r => r.Timestamp.Date == timestamp.Date);

        [EnableQuery]
        [HttpGet("GetSingleTest/{key}")]
        public SingleResult<BatchedTest> GetSingleTest([FromODataUri] int key)
        {
            var result = _context.TestResults.Where(b => b.ID == key);
            return SingleResult.Create(result);
        }
    }
}
