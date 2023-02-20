// RNG-Service - RNGController.cs
// Created on 2023.02.12
// Last modified at 2023.02.20 16:49

namespace RNG_Service.Controllers;

#region
using Microsoft.AspNetCore.Mvc;

using Models;

using System.Security.Cryptography;
#endregion

[ApiController]
[Route("RNG")]
public class RngController : ControllerBase
{
    private readonly ILogger<RngController> _logger;

    public RngController(ILogger<RngController> logger) => _logger = logger;

    [HttpGet("GetRandomFloat01")]
    public IActionResult GetRandomFloat01()
    {
        var number = Convert.ToDouble(RandomNumberGenerator.GetInt32(1000000000));
        var output = 1d - number / 1000000000;

        return Ok(output);
    }

    [HttpGet("TestRandomFloat01")]
    public IActionResult TestRandomFloat01()
    {
        TestResult testResult = new();

        for (var i = 0; i < 10000; i++)
        {
            var number = Convert.ToDouble(RandomNumberGenerator.GetInt32(1000000000));
            testResult.TestRun.Add(1d - number / 1000000000);
        }

        testResult.Mean = testResult.TestRun.Average();
        var sumOfSquaresOfDifferences = testResult.TestRun.Select(val => (val - testResult.Mean) * (val - testResult.Mean)).Sum();
        testResult.StandardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / testResult.TestRun.Count);

        return Ok(testResult);
    }
}
