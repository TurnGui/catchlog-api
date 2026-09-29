namespace CatchLog.Api.DTOs;

public class CreateCatchDto
{
    public int AnglerId { get; set; }
    public int SpeciesId { get; set; }
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