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
public class CatchesController : ControllerBase
{
    private readonly CatchService _catchService;

    public CatchesController(CatchService catchService)
    {
        _catchService = catchService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CatchResponseDto>>> GetAll()
    {
        var catches = await _catchService.GetAllAsync();
        return Ok(catches);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CatchResponseDto>> GetById(int id)
    {
        var fishCatch = await _catchService.GetByIdAsync(id);
        if (fishCatch is null)
        {
            return NotFound();
        }

        return Ok(fishCatch);
    }

    [HttpPost]
    public async Task<ActionResult<CatchResponseDto>> Create(CreateCatchDto dto)
    {
        try
        {
            var created = await _catchService.CreateAsync(User.GetAnglerId(), dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _catchService.DeleteAsync(id, User.GetAnglerId());
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
    }
}