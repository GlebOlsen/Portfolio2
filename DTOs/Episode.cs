namespace ImdbClone.Api.DTOs;

public class EpisodeDto
{
    public string Tconst { get; set; } = string.Empty;
    public string ParentTconst { get; set; } = string.Empty;
    public int? EpisodeNumber { get; set; }
    public int? SeasonNumber { get; set; }
    public string? ParentTitle { get; set; }
}
