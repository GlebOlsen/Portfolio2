namespace ImdbClone.Api.Entities;

public class SearchHistory
{
    public Guid HistoryId { get; set; }
    public Guid UserId { get; set; }
    public DateTime SearchDate { get; set; }
    public string SearchParameters { get; set; } = string.Empty;

    public ImdbUser? User { get; set; }
}