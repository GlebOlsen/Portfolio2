using System.ComponentModel.DataAnnotations.Schema;
using ImdbClone.Api.Enums;

namespace ImdbClone.Api.Entities;

public class TitlePerson
{
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("nconst")]
    public string Nconst { get; set; } = string.Empty;

    [Column("ordering")]
    public int Ordering { get; set; }

    [Column("category")]
    public PersonCategory? Category { get; set; }

    [Column("job")]
    public string? Job { get; set; }

    [Column("character_name")]
    public string? CharacterName { get; set; }

    [ForeignKey("Nconst")]
    public Person? Person { get; set; }

    [ForeignKey("Tconst")]
    public Title? Title { get; set; }
}
