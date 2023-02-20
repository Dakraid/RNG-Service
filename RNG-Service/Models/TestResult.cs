// RNG-Service - TestResult.cs
// Created on 2023.02.20
// Last modified at 2023.02.20 16:41

namespace RNG_Service.Models;

public class TestResult
{
    public double Mean { get; set; }
    public double StandardDeviation { get; set; }
    public List<double> TestRun { get; } = new();
}
