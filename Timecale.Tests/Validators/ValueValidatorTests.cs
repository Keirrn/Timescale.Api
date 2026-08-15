using Timecale.Application.DTOs;
using Timecale.Application.Exceptions;
using Timecale.Application.Validators;

namespace Timecale.Tests.Validators;

public class ValueValidatorTests
{
    private readonly ValueValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenValuesAreValid()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = DateTimeOffset.UtcNow.AddMinutes(-1),
                ExecutionTime = 1.5m,
                Value = 10m
            }
        };

        _validator.Validate(values);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenValuesAreEmpty()
    {
        var values = new List<ValueDTO>();

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenThereAreMoreThan10000Values()
    {
        var values = Enumerable.Range(1, 10001)
            .Select(_ => new ValueDTO
            {
                Date = DateTimeOffset.UtcNow,
                ExecutionTime = 1m,
                Value = 1m
            })
            .ToList();

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenDateIsBefore2000()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = new DateTimeOffset(
                    1999, 12, 31, 23, 59, 59, TimeSpan.Zero),
                ExecutionTime = 1m,
                Value = 1m
            }
        };

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenDateIsInTheFuture()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = DateTimeOffset.UtcNow.AddMinutes(1),
                ExecutionTime = 1m,
                Value = 1m
            }
        };

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenExecutionTimeIsNegative()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = DateTimeOffset.UtcNow,
                ExecutionTime = -1m,
                Value = 1m
            }
        };

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }

    [Fact]
    public void Validate_ShouldThrow_WhenValueIsNegative()
    {
        var values = new List<ValueDTO>
        {
            new()
            {
                Date = DateTimeOffset.UtcNow,
                ExecutionTime = 1m,
                Value = -1m
            }
        };

        Assert.Throws<ValidationException>(
            () => _validator.Validate(values));
    }
    [Fact]
    public void Validate_ShouldPass_WhenThereAreExactly10000Values()
    {
        var values = Enumerable.Range(1, 10000)
            .Select(_ => new ValueDTO
            {
                Date = DateTimeOffset.UtcNow,
                ExecutionTime = 1m,
                Value = 1m
            })
            .ToList();

        _validator.Validate(values);
    }
}