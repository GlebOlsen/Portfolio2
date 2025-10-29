using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class BookmarkTitle
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("bookmark_date")]
    public DateTime BookmarkDate { get; set; }

    // we alr have tconst and userid here but reference could be useful
    [ForeignKey("UserId")]
    public ImdbUser? User { get; set; }

    [ForeignKey("Tconst")]
    public Title? Title { get; set; }
}
