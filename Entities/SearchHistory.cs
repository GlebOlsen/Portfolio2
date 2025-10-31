using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class SearchHistory
{
    [Key]
    [Column("history_id")]
    public Guid HistoryId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("search_date")]
    public DateTime SearchDate { get; set; }

    [Column("search_parameters")]
    public string SearchParameters { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public ImdbUser? User { get; set; }
}
