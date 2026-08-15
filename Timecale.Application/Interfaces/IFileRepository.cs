using Timecale.Application.DTOs;
using Timecale.Domain.Entities;

namespace Timecale.Application.Interfaces;

public interface IFileRepository
{
    Task<UploadedFile?> GetByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken);

    Task AddAsync(
        UploadedFile file,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        UploadedFile file,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<ResultDTO>> GetResultsAsync(
        ResultFilterDTO filter,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<ValueResultDTO>> GetLastValuesAsync(
        string fileName,
        CancellationToken cancellationToken);
    Task<bool> ExistsByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken);
}