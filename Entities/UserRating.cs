using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class UserRating
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("rating")]
    public decimal Rating { get; set; }

    [Column("rating_date")]
    public DateTime RatingDate { get; set; }

    // we alr have tconst and userid here but reference could be useful

    [ForeignKey("UserId")]
    public ImdbUser? User { get; set; }

    [ForeignKey("Tconst")]
    public Title? Title { get; set; }
}
