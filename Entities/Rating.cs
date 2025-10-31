using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Rating
{
    [Key]
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("avg_rating")]
    public decimal? AvgRating { get; set; }

    [Column("num_votes")]
    public int? NumVotes { get; set; }

    [Column("metascore")]
    public int? MetaScore { get; set; }

    [ForeignKey("Tconst")]
    // we alr have the FK to title here (tconst) but a reference could be useful
    public Title? Title { get; set; }
}
