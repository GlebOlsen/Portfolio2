using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ImdbClone.Api.Enums;

namespace ImdbClone.Api.Entities;

[Table("title")]
public class Title
{
    [Key]
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("title_type")]
    public TitleType? TitleType { get; set; }

    [Column("primary_title")]
    public string? PrimaryTitle { get; set; }

    [Column("original_title")]
    public string? OriginalTitle { get; set; }

    [Column("is_adult")]
    public bool isAdult { get; set; }

    [Column("start_year")]
    public int? StartYear { get; set; }

    [Column("end_year")]
    public int? EndYear { get; set; }

    [Column("runtime_min")]
    public int? RunTimeMin { get; set; }

    [Column("rated")]
    public string? Rated { get; set; }

    [Column("plot")]
    public string? Plot { get; set; }

    [Column("poster")]
    public string? Poster { get; set; }

    [Column("award")]
    public string? Award { get; set; }

    public ICollection<Person> KnownForByPeople { get; set; } = new List<Person>();

    public ICollection<TitlePerson> TitlePeople { get; set; } = new List<TitlePerson>();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<Country> Countries { get; set; } = new List<Country>();
    public ICollection<BookmarkTitle> BookmarkTitles { get; set; } = new List<BookmarkTitle>();
    public ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
}
