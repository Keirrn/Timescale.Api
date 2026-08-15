using Timecale.Application.Calculators;
using Timecale.Application.DTOs;

namespace Timecale.Tests.Calculators;

public class ResultCalculatorTests
{
    private readonly ResultCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldCalculateAllResults()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = new DateTimeOffset(2026, 8, 14, 10, 0, 0, TimeSpan.Zero),
                ExecutionTime = 1.5m,
                Value = 10.5m
            },
            new()
            {
                Date = new DateTimeOffset(2026, 8, 14, 10, 0, 2, TimeSpan.Zero),
                ExecutionTime = 2m,
                Value = 20m
            },
            new()
            {
                Date = new DateTimeOffset(2026, 8, 14, 10, 0, 5, TimeSpan.Zero),
                ExecutionTime = 1m,
                Value = 15.5m
            },
            new()
            {
                Date = new DateTimeOffset(2026, 8, 14, 10, 0, 8, TimeSpan.Zero),
                ExecutionTime = 3m,
                Value = 25m
            },
            new()
            {
                Date = new DateTimeOffset(2026, 8, 14, 10, 0, 10, TimeSpan.Zero),
                ExecutionTime = 2.5m,
                Value = 30m
            }
        };

        var result = _calculator.Calculate(values);

        Assert.Equal(10m, result.TimeDelta);
        Assert.Equal(
            new DateTimeOffset(2026, 8, 14, 10, 0, 0, TimeSpan.Zero),
            result.FirstOperationDate);

        Assert.Equal(2.0m, result.AverageExecutionTime);
        Assert.Equal(20.2m, result.AverageValue);
        Assert.Equal(20m, result.MedianValue);
        Assert.Equal(30m, result.MaxValue);
        Assert.Equal(10.5m, result.MinValue);
    }

    [Fact]
    public void Calculate_ShouldCalculateMedianForEvenNumberOfValues()
    {
        var values = new List<ValueDTO>
        {
            new() { Date = DateTimeOffset.UtcNow, Value = 10m },
            new() { Date = DateTimeOffset.UtcNow, Value = 20m },
            new() { Date = DateTimeOffset.UtcNow, Value = 30m },
            new() { Date = DateTimeOffset.UtcNow, Value = 40m }
        };

        var result = _calculator.Calculate(values);

        Assert.Equal(25m, result.MedianValue);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenValuesAreEmpty()
    {
        var values = new List<ValueDTO>();

        Assert.Throws<Timecale.Application.Exceptions.ValidationException>(
            () => _calculator.Calculate(values));
    }
}