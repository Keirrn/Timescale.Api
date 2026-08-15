using Microsoft.AspNetCore.Mvc;
using Timecale.Application.DTOs;
using Timecale.Application.Exceptions;
using Timecale.Application.Interfaces;

namespace Timecale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileImportService _fileImportService;
    private readonly IValueService _valueService;

    public FilesController(
        IFileImportService fileImportService,
        IValueService valueService)
    {
        _fileImportService = fileImportService;
        _valueService = valueService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("Файл пуст.");
        }

        try
        {
            await using var stream = file.OpenReadStream();

            await _fileImportService.ImportAsync(
                file.FileName,
                stream,
                cancellationToken);

            return Ok("Файл успешно обработан");
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{fileName}/values")]
    public async Task<ActionResult<IReadOnlyList<ValueResultDTO>>> GetLastValues(
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var values = await _valueService.GetLastValuesAsync(
                fileName,
                cancellationToken);

            return Ok(values);
        }
        catch (FileNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}