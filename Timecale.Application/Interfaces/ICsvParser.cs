using Timecale.Application.DTOs;
namespace Timecale.Application.Interfaces;

public interface ICsvParser
{
    Task<IReadOnlyList<ValueDTO>> ParseAsync(
        Stream fileStream,
        CancellationToken cancellationToken);
}