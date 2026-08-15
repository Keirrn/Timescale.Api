using Timecale.Application.DTOs;

namespace Timecale.Application.Interfaces;

public interface IValueValidator
{
    void Validate(IReadOnlyList<ValueDTO> values);
}