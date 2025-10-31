namespace ImdbClone.Api.DTOs;

public class RatingDto
{
    public string Tconst { get; set; } = string.Empty;
    public decimal? AvgRating { get; set; }
    public int? NumVotes { get; set; }
    public int? MetaScore { get; set; }
    public string? TitleName { get; set; }
}
