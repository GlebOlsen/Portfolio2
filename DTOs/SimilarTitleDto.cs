using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.DTOs;

public class SimilarTitleDto
{
    [Column("tconst")]
    public string Tconst { get; set; } = null!;

    [Column("primary_title")]
    public string PrimaryTitle { get; set; } = null!;

    [Column("similarity_score")]
    public long SimilarityScore { get; set; }

    [Column("avg_rating")]
    public decimal AvgRating { get; set; }

    [Column("num_votes")]
    public int NumVotes { get; set; }
}
