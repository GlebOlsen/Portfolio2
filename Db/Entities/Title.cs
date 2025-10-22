using IMDB.DataService.Db.Enums;
namespace IMDB.DataService.Db.Entities;

public class Title
{
    public string Tconst { get; set; } = string.Empty;
    public TitleType? TitleType { get; set; }

    public string? PrimaryTitle { get; set; }

    public string? OriginalTitle { get; set; }

    public bool isAdult { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public int? RunTimeMin { get; set; }

    public string? Rated { get; set; }

    public string? Plot { get; set; }

    public string? Poster { get; set; }

    public string? Award { get; set; }

    public ICollection<Person> KnownForByPeople { get; set; } = new List<Person>();

    public List<TitlePerson> TitlePeople { get; set; } = new();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<Country> Countries { get; set; } = new List<Country>();
}