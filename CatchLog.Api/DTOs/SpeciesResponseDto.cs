namespace CatchLog.Api.DTOs;

public class SpeciesResponseDto
{
    public int Id { get; set; }
    public string CommonName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public bool IsProtected { get; set; }
}