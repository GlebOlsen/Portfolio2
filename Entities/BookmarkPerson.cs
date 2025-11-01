using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class BookmarkPerson
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("nconst")]
    public string Nconst { get; set; } = string.Empty;

    [Column("bookmark_date")]
    public DateTime BookmarkDate { get; set; }

    [ForeignKey("UserId")]
    public ImdbUser? User { get; set; }

    [ForeignKey("Nconst")]
    public Person? Person { get; set; }
}
