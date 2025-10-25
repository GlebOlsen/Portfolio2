namespace ImdbClone.Api.Entities;

public class BookmarkPerson
{
    public Guid UserId { get; set; }
    public string Nconst { get; set; } = string.Empty;
    public DateTime BookmarkDate { get; set; }

    public ImdbUser? User { get; set; }

    public Person? Person { get; set; }
}