namespace CatchLog.Api.DTOs;

public class CreateSpeciesDto
{
     public string CommonName { get; set; } = string.Empty;
     public string ScientificName { get; set; } = string.Empty;
     public bool IsProtected { get; set; }
}