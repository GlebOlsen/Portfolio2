namespace IMDB.DataService.Db.Entities;

public class Episode
{
    public string Tconst { get; set; } = string.Empty;
    public string ParentTconst { get; set; } = string.Empty;
    public int? EpisodeNumber { get; set; }
    public int? SeasonNumber { get; set; }

    // we alr have the FK to title here (tconst) but a reference could be useful
    public Title? Title { get; set; }
    public Title? ParentTitle { get; set; }
}