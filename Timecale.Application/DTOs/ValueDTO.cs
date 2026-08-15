namespace Timecale.Application.DTOs;

public class  ValueDTO
{
    public DateTimeOffset Date { get; set; }

    public decimal ExecutionTime { get; set; }

    public decimal Value { get; set; }
}