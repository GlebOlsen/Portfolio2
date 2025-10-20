namespace IMDB.DataService.Db.Entities;

public class TitleAlias
{
    public string Tconst { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public string AltTitle { get; set; } = string.Empty;
    public string? Types { get; set; }
    public string? Language { get; set; }
    public string? Region { get; set; }
    public string? Attributes { get; set; }
    // we alr have the FK to title here (tconst) but a reference could be useful
    public Title? Title { get; set; }
}