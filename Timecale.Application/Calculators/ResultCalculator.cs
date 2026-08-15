using Timecale.Application.DTOs;
using Timecale.Application.Exceptions;
using Timecale.Application.Interfaces;
using Timecale.Domain.Entities;

namespace Timecale.Application.Calculators;

public class ResultCalculator : IResultCalculator
{
    public Result Calculate(IReadOnlyList<ValueDTO> values)
    {
        if (values.Count == 0)
        {
            throw new ValidationException(
                "Файл пустой");
        }

        var firstDate = values.Min(x => x.Date);
        var lastDate = values.Max(x => x.Date);

        var timeDelta = (decimal)(lastDate - firstDate).TotalSeconds;

        var averageExecutionTime =
            values.Average(x => x.ExecutionTime);

        var averageValue =
            values.Average(x => x.Value);

        var sortedValues = values
            .Select(x => x.Value)
            .OrderBy(x => x)
            .ToList();

        var medianValue = CalculateMedian(sortedValues);

        var maxValue =
            values.Max(x => x.Value);

        var minValue =
            values.Min(x => x.Value);

        return new Result
        {
            TimeDelta = timeDelta,
            FirstOperationDate = firstDate,
            AverageExecutionTime = averageExecutionTime,
            AverageValue = averageValue,
            MedianValue = medianValue,
            MaxValue = maxValue,
            MinValue = minValue
        };
    }

    private static decimal CalculateMedian(
        IReadOnlyList<decimal> sortedValues)
    {
        var middle = sortedValues.Count / 2;

        if (sortedValues.Count % 2 == 0)
        {
            return (sortedValues[middle - 1] + sortedValues[middle]) / 2;
        }

        return sortedValues[middle];
    }
}