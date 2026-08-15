namespace Timecale.Application.DTOs;

public class ResultFilterDTO
{
    public string? FileName { get; set; }

    public DateTimeOffset? FirstOperationDateFrom { get; set; }

    public DateTimeOffset? FirstOperationDateTo { get; set; }

    public decimal? AverageValueFrom { get; set; }

    public decimal? AverageValueTo { get; set; }

    public decimal? AverageExecutionTimeFrom { get; set; }

    public decimal? AverageExecutionTimeTo { get; set; }
}