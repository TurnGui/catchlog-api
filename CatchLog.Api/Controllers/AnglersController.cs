using CatchLog.Api.DTOs;
using CatchLog.Api.Exceptions;
using CatchLog.Api.Extensions;
using CatchLog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchLog.Api.Controllers;

[ApiController]
[Authorize]
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
    [AllowAnonymous]
    public async Task<ActionResult<AnglerResponseDto>> Create(CreateAnglerDto dto)
    {
        var created = await _anglerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AnglerResponseDto>> Update(int id, UpdateAnglerDto dto)
    {
        if (id != User.GetAnglerId())
        {
            throw new ForbiddenException("You can only update your own account.");
        }

        var updated = await _anglerService.UpdateAsync(id, dto);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id != User.GetAnglerId())
        {
            throw new ForbiddenException("You can only delete your own account.");
        }

        var deleted = await _anglerService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}