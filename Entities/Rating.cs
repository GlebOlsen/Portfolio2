namespace ImdbClone.Api.Entities;

public class Rating
{
    public string Tconst { get; set; } = string.Empty;
    public decimal? AvgRating { get; set; }
    public int? NumVotes { get; set; }
    public int? MetaScore { get; set; }

    // we alr have the FK to title here (tconst) but a reference could be useful
    public Title? Title { get; set; }
}
