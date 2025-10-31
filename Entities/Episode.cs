using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Episode
{
    [Key]
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("parent_tconst")]
    public string ParentTconst { get; set; } = string.Empty;

    [Column("episode_number")]
    public int? EpisodeNumber { get; set; }

    [Column("season_number")]
    public int? SeasonNumber { get; set; }

    [ForeignKey("Tconst")]
    public Title? ParentTitle { get; set; }
}
