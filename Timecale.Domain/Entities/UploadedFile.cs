namespace Timecale.Domain.Entities;

public class UploadedFile
{
    public long Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public ICollection<Value> Values { get; set; } = new List<Value>();

    public Result? Result { get; set; }
}