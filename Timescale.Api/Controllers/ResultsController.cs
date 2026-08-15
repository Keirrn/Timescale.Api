using Microsoft.AspNetCore.Mvc;
using Timecale.Application.DTOs;
using Timecale.Application.Interfaces;

namespace Timecale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultDTO>>> GetResults(
        [FromQuery] ResultFilterDTO filter,
        CancellationToken cancellationToken)
    {
        var results = await _resultService.GetResultsAsync(
            filter,
            cancellationToken);

        return Ok(results);
    }
}