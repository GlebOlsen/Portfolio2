using ImdbClone.Api.Entities;
using ImdbClone.Api.Enums;

namespace ImdbClone.Api.DTOs;

public class TitlePersonDto
{
    public string Nconst { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? CharacterName { get; set; }
}

public class PersonFullDto
{
    public string Nconst { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public decimal? DerivedRating { get; set; }
    public List<Profession> Professions { get; set; } = new();
}

public class PersonSearchResultDto
{
    public string Nconst { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
}

public class PersonListDto
{
    public string Nconst { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Professions { get; set; } = new();
}

public class PersonWithProfessionDto
{
    public string Nconst { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public List<Profession> Professions { get; set; } = new();
}
