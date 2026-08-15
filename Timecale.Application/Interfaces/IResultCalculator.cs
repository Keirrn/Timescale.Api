using Timecale.Application.DTOs;
using Timecale.Domain.Entities;

namespace Timecale.Application.Interfaces;

public interface IResultCalculator
{
    Result Calculate(IReadOnlyList<ValueDTO> values);
}