using System.Text.Json;
using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace CatchLog.Api.Services;

public class StatisticsService
{
    public const string SpeciesStatisticsCacheKey = "statistics:species";

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    private readonly CatchLogDbContext _context;
    private readonly IDistributedCache _cache;

    public StatisticsService(CatchLogDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<SpeciesStatisticsDto>> GetSpeciesStatisticsAsync()
    {
        var cached = await _cache.GetStringAsync(SpeciesStatisticsCacheKey);
        if (cached is not null)
        {
            return JsonSerializer.Deserialize<List<SpeciesStatisticsDto>>(cached)!;
        }

        var statistics = await _context.Catches
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

        await _cache.SetStringAsync(
            SpeciesStatisticsCacheKey,
            JsonSerializer.Serialize(statistics),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration
            });

        return statistics;
    }
}