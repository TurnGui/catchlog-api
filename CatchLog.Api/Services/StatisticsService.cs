using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Services;

public class StatisticsService
{
    private readonly CatchLogDbContext _context;

    public StatisticsService(CatchLogDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpeciesStatisticsDto>> GetSpeciesStatisticsAsync()
    {
        return await _context.Catches
            .GroupBy(c => new { c.SpeciesId, c.Species.CommonName })
            .OrderByDescending(g => g.Count())
            .Select(g => new SpeciesStatisticsDto
            {
                SpeciesId = g.Key.SpeciesId,
                CommonName = g.Key.CommonName,
                TotalCatches = g.Count(),
                TotalWeightKg = g.Sum(c => c.WeightKg),
                HeaviestCatchKg = g.Max(c => c.WeightKg)
            })
            .ToListAsync();
    }
}