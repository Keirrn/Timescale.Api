namespace Timecale.Domain.Entities;

public class Value
{
    public long Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public decimal ExecutionTime { get; set; }

    public decimal MeasurementValue { get; set; }

    public long FileId { get; set; }
    public UploadedFile File { get; set; } = null!;
}