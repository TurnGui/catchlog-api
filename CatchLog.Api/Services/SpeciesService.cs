using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using CatchLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Services;

public class SpeciesService
{
    private readonly CatchLogDbContext _context;

    public SpeciesService(CatchLogDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpeciesResponseDto>> GetAllAsync()
    {
        return await _context.Species
            .Select(s => ToResponseDto(s))
            .ToListAsync();
    }

     public async Task<SpeciesResponseDto?> GetByIdAsync(int id)
    {
        var species = await _context.Species.FindAsync(id);
        return species is null ? null : ToResponseDto(species);
    }
       public async Task<SpeciesResponseDto> CreateAsync(CreateSpeciesDto dto)
    {
        var species = new Species
        {
            CommonName = dto.CommonName,
            ScientificName = dto.ScientificName,
            IsProtected = dto.IsProtected
        };

        _context.Species.Add(species);
        await _context.SaveChangesAsync();

        return ToResponseDto(species);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var species = await _context.Species.FindAsync(id);
        if (species is null)
        {
            return false;
        }

        _context.Species.Remove(species);
        await _context.SaveChangesAsync();
        return true;
    }

    private static SpeciesResponseDto ToResponseDto(Species species)
    {
        return new SpeciesResponseDto
        {
            Id = species.Id,
            CommonName = species.CommonName,
            ScientificName = species.ScientificName,
            IsProtected = species.IsProtected
        };
    }
}
