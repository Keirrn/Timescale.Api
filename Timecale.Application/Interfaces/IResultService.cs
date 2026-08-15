using Timecale.Application.DTOs;

namespace Timecale.Application.Interfaces;

public interface IResultService
{
    Task<IReadOnlyList<ResultDTO>> GetResultsAsync(
        ResultFilterDTO filter,
        CancellationToken cancellationToken);
}