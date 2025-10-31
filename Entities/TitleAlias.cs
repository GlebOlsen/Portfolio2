using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class TitleAlias
{
    [Key]
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("ordering")]
    public int Ordering { get; set; }

    [Column("alt_title")]
    public string AltTitle { get; set; } = string.Empty;

    [Column("types")]
    public string? Types { get; set; }

    [Column("language")]
    public string? Language { get; set; }

    [Column("region")]
    public string? Region { get; set; }

    [Column("attributes")]
    public string? Attributes { get; set; }

    // we alr have the FK to title here (tconst) but a reference could be useful
    [ForeignKey("Tconst")]
    public Title? Title { get; set; }
}
