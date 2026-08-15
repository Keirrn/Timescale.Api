using Timecale.Application.Interfaces;
using Timecale.Domain.Entities;

namespace Timecale.Application.Services;

public class FileImportService : IFileImportService
{
    private readonly IFileRepository _fileRepository;
    private readonly ICsvParser _csvParser;
    private readonly IValueValidator _valueValidator;
    private readonly IResultCalculator _resultCalculator;
    private readonly ITransaction _transaction;

    public FileImportService(
        IFileRepository fileRepository,
        ICsvParser csvParser,
        IValueValidator valueValidator,
        IResultCalculator resultCalculator,
        ITransaction transaction)
    {
        _fileRepository = fileRepository;
        _csvParser = csvParser;
        _valueValidator = valueValidator;
        _resultCalculator = resultCalculator;
        _transaction = transaction;
    }

    public async Task ImportAsync(
        string fileName,
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        var values = await _csvParser.ParseAsync(
            fileStream,
            cancellationToken);

        _valueValidator.Validate(values);

        var result = _resultCalculator.Calculate(values);

        var existingFile = await _fileRepository.GetByFileNameAsync(
            fileName,
            cancellationToken);

        await _transaction.BeginTransactionAsync(
            cancellationToken);

        try
        {
            if (existingFile is not null)
            {
                await _fileRepository.DeleteAsync(
                    existingFile,
                    cancellationToken);
            }

            var file = new UploadedFile
            {
                FileName = fileName
            };

            foreach (var value in values)
            {
                file.Values.Add(new Value
                {
                    Date = value.Date,
                    ExecutionTime = value.ExecutionTime,
                    MeasurementValue = value.Value
                });
            }

            file.Result = result;

            await _fileRepository.AddAsync(
                file,
                cancellationToken);

            await _transaction.SaveChangesAsync(
                cancellationToken);

            await _transaction.CommitTransactionAsync(
                cancellationToken);
        }
        catch
        {
            await _transaction.RollbackTransactionAsync(
                cancellationToken);

            throw;
        }
    }
}