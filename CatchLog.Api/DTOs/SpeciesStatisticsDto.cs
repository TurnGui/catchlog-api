namespace CatchLog.Api.DTOs;

public class SpeciesStatisticsDto
{
    public int SpeciesId { get; set; }
    public string CommonName { get; set; } = string.Empty;
    public int TotalCatches { get; set; }
    public decimal TotalWeightKg { get; set; }
    public decimal HeaviestCatchKg { get; set; }
}