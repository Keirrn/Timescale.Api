using Timecale.Application.DTOs;

namespace Timecale.Application.Interfaces;

public interface IValueService
{
    Task<IReadOnlyList<ValueResultDTO>> GetLastValuesAsync(
        string fileName,
        CancellationToken cancellationToken);
}