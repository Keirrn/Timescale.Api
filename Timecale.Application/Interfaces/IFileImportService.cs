namespace Timecale.Application.Interfaces;

public interface IFileImportService
{
    Task ImportAsync(
        string fileName,
        Stream fileStream,
        CancellationToken cancellationToken);
}