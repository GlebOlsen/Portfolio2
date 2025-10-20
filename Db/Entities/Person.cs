namespace IMDB.DataService.Db.Entities;

public class Person
{
    public string Nconst { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public decimal? DerivedRating { get; set; }

    public ICollection<Title> KnownForTitles { get; set; } = new List<Title>();

    public ICollection<Profession> Professions { get; set; } = new List<Profession>();
}