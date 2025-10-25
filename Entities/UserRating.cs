namespace ImdbClone.Api.Entities;

public class UserRating
{
    public Guid UserId { get; set; }
    public string Tconst { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public DateTime RatingDate { get; set; }

    // we alr have tconst and userid here but reference could be useful
    public ImdbUser? User { get; set; }
    public Title? Title { get; set; }
}