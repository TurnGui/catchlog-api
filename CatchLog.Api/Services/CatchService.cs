using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using CatchLog.Api.Exceptions;
using CatchLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Services;

public class CatchService
{
    private readonly CatchLogDbContext _context;

    public CatchService(CatchLogDbContext context)
    {
        _context = context;
    }

    public async Task<List<CatchResponseDto>> GetAllAsync()
    {
        var catches = await _context.Catches
            .Include(c => c.Angler)
            .Include(c => c.Species)
            .ToListAsync();

        return catches.Select(ToResponseDto).ToList();
    }

    public async Task<CatchResponseDto?> GetByIdAsync(int id)
    {
        var fishCatch = await _context.Catches
            .Include(c => c.Angler)
            .Include(c => c.Species)
            .FirstOrDefaultAsync(c => c.Id == id);

        return fishCatch is null ? null : ToResponseDto(fishCatch);
    }

    public async Task<CatchResponseDto> CreateAsync(int anglerId, CreateCatchDto dto)
    {
        var angler = await _context.Anglers.FindAsync(anglerId)
            ?? throw new NotFoundException("Angler", anglerId);

        var species = await _context.Species.FindAsync(dto.SpeciesId)
            ?? throw new NotFoundException("Species", dto.SpeciesId);

        if (species.IsProtected && !dto.IsCatchAndRelease)
        {
            throw new BusinessRuleException(
                $"{species.CommonName} is a protected species and can only be registered as catch and release.");
        }

        var fishCatch = new Catch
        {
            Angler = angler,
            Species = species,
            WeightKg = dto.WeightKg,
            LengthCm = dto.LengthCm,
            Location = dto.Location,
            CaughtAt = dto.CaughtAt,
            Notes = dto.Notes,
            BaitUsed = dto.BaitUsed,
            WaterConditions = dto.WaterConditions,
            IsCatchAndRelease = dto.IsCatchAndRelease,
            PhotoUrl = dto.PhotoUrl
        };

        _context.Catches.Add(fishCatch);
        await _context.SaveChangesAsync();

        return ToResponseDto(fishCatch);
    }

    public async Task<bool> DeleteAsync(int id, int anglerId)
    {
        var fishCatch = await _context.Catches.FindAsync(id);
        if (fishCatch is null)
        {
            return false;
        }

        if (fishCatch.AnglerId != anglerId)
        {
            throw new ForbiddenException("You can only delete your own catches.");
        }

        _context.Catches.Remove(fishCatch);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CatchResponseDto ToResponseDto(Catch fishCatch)
    {
        return new CatchResponseDto
        {
            Id = fishCatch.Id,
            AnglerId = fishCatch.AnglerId,
            AnglerName = fishCatch.Angler.Name,
            SpeciesId = fishCatch.SpeciesId,
            SpeciesCommonName = fishCatch.Species.CommonName,
            WeightKg = fishCatch.WeightKg,
            LengthCm = fishCatch.LengthCm,
            Location = fishCatch.Location,
            CaughtAt = fishCatch.CaughtAt,
            Notes = fishCatch.Notes,
            BaitUsed = fishCatch.BaitUsed,
            WaterConditions = fishCatch.WaterConditions,
            IsCatchAndRelease = fishCatch.IsCatchAndRelease,
            PhotoUrl = fishCatch.PhotoUrl
        };
    }
}