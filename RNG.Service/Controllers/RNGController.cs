#region header
// RNG.Service
// RNG.Service / RNGController.cs BY Kristian Schlikow
// First modified on 2023.02.21
// Last modified on 2023.02.27
#endregion

namespace RNG.Service.Controllers
{
#region usings
#region usings
    using Database;

    using Microsoft.AspNetCore.Mvc;

    using Models;

    using System.Security.Cryptography;
    #endregion
    #endregion

    [ApiController]
    [Route("api/[controller]")]
    public class RngController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<RngController> _logger;

        public RngController(ILogger<RngController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetRandomFloat01")]
        public async Task<IActionResult> GetRandomFloat01()
        {
            var referer = Request.Headers.Referer;

            var randomSequence = RandomNumberGenerator.GetBytes(8);
            var asLong = BitConverter.ToInt64(randomSequence, 0);
            var number = (double)(asLong & long.MaxValue) / long.MaxValue;

            var rngResult = new RngEntry
            {
                Result = number, Requestor = referer.ToString(), Timestamp = DateTime.UtcNow
            };

            await _context.RngResults.AddAsync(rngResult);
            await _context.SaveChangesAsync();

            return Ok(number);
        }

        [HttpGet("TestRandomFloat01")]
        public async Task<IActionResult> TestRandomFloat01()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var testResult = new BatchedTest
            {
                Timestamp = DateTime.UtcNow
            };

            var testList = new List<RngEntry>();

            for (var i = 0; i < 1000; i++)
            {
                var randomSequence = RandomNumberGenerator.GetBytes(8);
                var asLong = BitConverter.ToInt64(randomSequence, 0);
                var number = (double)(asLong & long.MaxValue) / long.MaxValue;

                testList.Add(new RngEntry
                {
                    Requestor = "Automated Test", Result = number, Timestamp = DateTime.UtcNow
                });
            }

            testResult.Mean = testList.Average(x => x.Result);
            var sumOfSquaresOfDifferences = testList.Select(val => (val.Result - testResult.Mean) * (val.Result - testResult.Mean)).Sum();
            testResult.StandardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / testList.Count);

            testResult.RngList = testList;
            await _context.TestResults.AddAsync(testResult);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(testResult);
        }
    }
}
