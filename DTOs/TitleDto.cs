namespace ImdbClone.Api.DTOs;

using System.ComponentModel.DataAnnotations.Schema;
using ImdbClone.Api.Enums;

public class TitleListDto
{
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public int? StartYear { get; set; }
    public string? Plot { get; set; }
    public TitleType? Type { get; set; }
    public string? PosterUrl { get; set; }
}

public class TitleFullDto
{
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? Type { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }

    [Column("runtime_min")]
    public int? RuntimeMin { get; set; }
    public string? Rated { get; set; }
    public string? Plot { get; set; }
    public string? PosterUrl { get; set; }
    public string? Award { get; set; }
    public List<string> Genres { get; set; } = new();
    public List<string?> Countries { get; set; }
    public List<TitlePersonDto> People { get; set; } = new();
    public bool IsBookmarked { get; set; }
    public int? UserRating { get; set; }
}

public class TitleRatingListDto
{
    public string Tconst { get; set; }
    public string PrimaryTitle { get; set; }
    public int Rating { get; set; }
    public DateTime RatingDate { get; set; }
}

public class TitleSearchResultDto
{
    [Column("tconst")]
    public string Tconst { get; set; } = string.Empty;

    [Column("primary_title")]
    public string PrimaryTitle { get; set; } = string.Empty;
}
