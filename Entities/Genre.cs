using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Genre
{
    [Key]
    [Column("genre_id")]
    public Guid GenreId { get; set; }

    [Column("genre_name")]
    public string GenreName { get; set; } = string.Empty;

    public ICollection<Title> Titles { get; set; } = new List<Title>();
}
