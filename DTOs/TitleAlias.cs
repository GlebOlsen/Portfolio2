namespace ImdbClone.Api.DTOs;

public class TitleAliasDto
{
    public string Tconst { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public string AltTitle { get; set; } = string.Empty;
    public string? Types { get; set; }
    public string? Language { get; set; }
    public string? Region { get; set; }
    public string? Attributes { get; set; }
}
