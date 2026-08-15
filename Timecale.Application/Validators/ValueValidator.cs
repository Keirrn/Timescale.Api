using Timecale.Application.DTOs;
using Timecale.Application.Exceptions;
using Timecale.Application.Interfaces;

namespace Timecale.Application.Validators;

public class ValueValidator : IValueValidator
{
    private static readonly DateTimeOffset MinDate =
        new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public void Validate(IReadOnlyList<ValueDTO> values)
    {
        if (values.Count is < 1 or > 10_000)
        {
            throw new ValidationException(
                "Количество записей должно быть от 1 до 10 000");
        }

        var now = DateTimeOffset.UtcNow;

        foreach (var value in values)
        {
            if (value.Date < MinDate)
            {
                throw new ValidationException(
                    $"Дата не может быть раньше 01.01.2000: {value.Date}");
            }

            if (value.Date > now)
            {
                throw new ValidationException(
                    $"Дата не может быть позже текущего времени: {value.Date}");
            }

            if (value.ExecutionTime < 0)
            {
                throw new ValidationException(
                    "Время выполнения не может быть отрицательным");
            }

            if (value.Value < 0)
            {
                throw new ValidationException(
                    "Значение показателя не может быть отрицательным");
            }
        }
    }
}