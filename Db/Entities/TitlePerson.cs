using IMDB.DataService.Db.Enums;

namespace IMDB.DataService.Db.Entities;

public class TitlePerson
{
    public string Tconst { get; set; } = string.Empty;
    public string Nconst { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public PersonCategory? Category { get; set; }
    public string? Job { get; set; }
    public string? CharacterName { get; set; }

    // we alr have the FK to title and person here (tconst & nconst) but a reference could be useful
    public Person? Person { get; set; }
    public Title? Title { get; set; }
}