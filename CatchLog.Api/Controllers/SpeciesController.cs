using CatchLog.Api.DTOs;
using CatchLog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeciesController : ControllerBase
{
    private readonly SpeciesService _speciesService;

    public SpeciesController(SpeciesService speciesService)
    {
        _speciesService = speciesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpeciesResponseDto>>> GetAll()
    {
        var species = await _speciesService.GetAllAsync();
        return Ok(species);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpeciesResponseDto>> GetById(int id)
    {
        var species = await _speciesService.GetByIdAsync(id);
        if (species is null)
        {
            return NotFound();
        }

        return Ok(species);
    }

    [HttpPost]
    public async Task<ActionResult<SpeciesResponseDto>> Create(CreateSpeciesDto dto)
    {
        var created = await _speciesService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _speciesService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}