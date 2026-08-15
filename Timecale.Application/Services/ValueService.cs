using Timecale.Application.DTOs;
using Timecale.Application.Interfaces;

namespace Timecale.Application.Services;

public class ValueService : IValueService
{
    private readonly IFileRepository _fileRepository;

    public ValueService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IReadOnlyList<ValueResultDTO>> GetLastValuesAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        var fileExists = await _fileRepository.ExistsByFileNameAsync(
            fileName,
            cancellationToken);

        if (!fileExists)
        {
            throw new FileNotFoundException(
                $"Файл с именем '{fileName}' не найден. " +
                "Возможно, вы забыли указать расширение файла, например '.csv'.");
        }

        return await _fileRepository.GetLastValuesAsync(
            fileName,
            cancellationToken);
    }
}