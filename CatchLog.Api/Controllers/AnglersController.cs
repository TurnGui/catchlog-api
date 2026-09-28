using CatchLog.Api.DTOs;
using CatchLog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnglersController : ControllerBase
{
    private readonly AnglerService _anglerService;

    public AnglersController(AnglerService anglerService)
    {
        _anglerService = anglerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnglerResponseDto>>> GetAll()
    {
        var anglers = await _anglerService.GetAllAsync();
        return Ok(anglers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnglerResponseDto>> GetById(int id)
    {
        var angler = await _anglerService.GetByIdAsync(id);
        if (angler is null)
        {
            return NotFound();
        }

        return Ok(angler);
    }

    [HttpPost]
    public async Task<ActionResult<AnglerResponseDto>> Create(CreateAnglerDto dto)
    {
        var created = await _anglerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _anglerService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}