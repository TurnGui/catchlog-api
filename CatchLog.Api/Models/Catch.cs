namespace CatchLog.Api.Models;

public class Catch
{
    public int Id { get; set; }

    public int AnglerId { get; set; }
    public Angler Angler { get; set; } = null!;

    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;

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