using CatchLog.Api.DTOs;
using CatchLog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;

    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("species")]
    public async Task<ActionResult<List<SpeciesStatisticsDto>>> GetSpeciesStatistics()
    {
        var statistics = await _statisticsService.GetSpeciesStatisticsAsync();
        return Ok(statistics);
    }
}