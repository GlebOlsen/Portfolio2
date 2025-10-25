namespace ImdbClone.Api.Entities;

public class Genre
{
    public Guid GenreId { get; set; }
    public string GenreName { get; set; } = string.Empty;

    public ICollection<Title> Titles { get; set; } = new List<Title>();
}