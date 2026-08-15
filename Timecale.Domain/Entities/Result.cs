namespace Timecale.Domain.Entities;

public class Result
{
    public long Id { get; set; }

    public long FileId { get; set; }

    public decimal TimeDelta { get; set; }

    public DateTimeOffset FirstOperationDate { get; set; }

    public decimal AverageExecutionTime { get; set; }

    public decimal AverageValue { get; set; }

    public decimal MedianValue { get; set; }

    public decimal MaxValue { get; set; }

    public decimal MinValue { get; set; }
    public UploadedFile File { get; set; } = null!;
}