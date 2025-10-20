namespace IMDB.DataService.Db.Entities;

public class BookmarkTitle
{
    public Guid UserId { get; set; }
    public string Tconst { get; set; } = string.Empty;
    public DateTime BookmarkDate { get; set; }

    // we alr have tconst and userid here but reference could be useful
    public ImdbUser? User { get; set; }
    public Title? Title { get; set; }
}