namespace Timecale.Application.DTOs;

public class ResultDTO
{
    public string FileName { get; set; } = string.Empty;

    public decimal TimeDelta { get; set; }

    public DateTimeOffset FirstOperationDate { get; set; }

    public decimal AverageExecutionTime { get; set; }

    public decimal AverageValue { get; set; }

    public decimal MedianValue { get; set; }

    public decimal MaxValue { get; set; }

    public decimal MinValue { get; set; }
}