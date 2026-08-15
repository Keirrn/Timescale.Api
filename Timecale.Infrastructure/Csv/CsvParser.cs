using System.Globalization;
using Timecale.Application.DTOs;
using Timecale.Application.Interfaces;

namespace Timecale.Infrastructure.Csv;

public class CsvParser : ICsvParser
{
    public async Task<IReadOnlyList<ValueDTO>> ParseAsync(
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(fileStream);

        var records = new List<ValueDTO>();

        var header = await reader.ReadLineAsync(cancellationToken);

        if (header is null)
        {
            throw new FormatException("Файл пуст");
        }

        if (header != "Date;ExecutionTime;Value")
        {
            throw new FormatException(
                "Некорректный заголовок файла");
        }

        string? line;

        while ((line = await reader.ReadLineAsync(cancellationToken)) is not null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new FormatException(
                    "Файл содержит пустую строку");
            }

            var columns = line.Split(';');

            if (columns.Length != 3)
            {
                throw new FormatException(
                    "CSV-запись должна содержать 3 значения");
            }

            if (string.IsNullOrWhiteSpace(columns[0]) ||
                string.IsNullOrWhiteSpace(columns[1]) ||
                string.IsNullOrWhiteSpace(columns[2]))
            {
                throw new FormatException(
                    "Все значения CSV-записи обязательны");
            }

            if (!DateTimeOffset.TryParse(
                    columns[0],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal |
                    DateTimeStyles.AdjustToUniversal,
                    out var date))
            {
                throw new FormatException(
                    $"Некорректная дата: {columns[0]}");
            }

            if (!decimal.TryParse(
                    columns[1],
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var executionTime))
            {
                throw new FormatException(
                    $"Некорректное время выполнения: {columns[1]}");
            }

            if (!decimal.TryParse(
                    columns[2],
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var value))
            {
                throw new FormatException(
                    $"Некорректное значение: {columns[2]}");
            }

            records.Add(new ValueDTO()
            {
                Date = date,
                ExecutionTime = executionTime,
                Value = value
            });
        }

        return records;
    }
}