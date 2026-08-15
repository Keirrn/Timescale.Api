using Timecale.Application.DTOs;
using Timecale.Application.Interfaces;

namespace Timecale.Application.Services;

public class ResultService : IResultService
{
    private readonly IFileRepository _fileRepository;

    public ResultService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IReadOnlyList<ResultDTO>> GetResultsAsync(
        ResultFilterDTO filter,
        CancellationToken cancellationToken)
    {
        return await _fileRepository.GetResultsAsync(
            filter,
            cancellationToken);
    }
}