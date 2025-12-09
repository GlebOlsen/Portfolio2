using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Person
{
    [Key]
    [Column("nconst")]
    public string Nconst { get; set; } = string.Empty;

    [Column("full_name")]
    public string? FullName { get; set; }

    [Column("birth_year")]
    public int? BirthYear { get; set; }

    [Column("death_year")]
    public int? DeathYear { get; set; }

    [Column("derived_rating")]
    public decimal? DerivedRating { get; set; }

    public ICollection<Title> KnownForTitles { get; set; } = new List<Title>();

    public ICollection<Profession> Professions { get; set; } = new List<Profession>();

    public ICollection<BookmarkPerson> BookmarkPersons { get; set; } = new List<BookmarkPerson>();
}
