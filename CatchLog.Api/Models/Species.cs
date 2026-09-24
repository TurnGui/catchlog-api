namespace CatchLog.Api.Models;

public class Species{
    public int Id { get; set; }
    public string CommonName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public bool IsProtected { get; set; }

    public ICollection<Catch> Catches { get; set; } = new List<Catch>();
}