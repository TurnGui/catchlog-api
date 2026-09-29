namespace CatchLog.Api.DTOs;

public class CatchResponseDto
{
    public int Id { get; set; }
    public int AnglerId { get; set; }
    public string AnglerName { get; set; } = string.Empty;
    public int SpeciesId { get; set; }
    public string SpeciesCommonName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal LengthCm { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime CaughtAt { get; set; }
    public string? Notes { get; set; }
    public string? BaitUsed { get; set; }
    public string? WaterConditions { get; set; }
    public bool IsCatchAndRelease { get; set; }
    public string? PhotoUrl { get; set; }
}