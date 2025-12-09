using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class ImdbUser
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("salt")]
    public string Salt { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<BookmarkTitle> BookmarkTitles { get; set; } = new List<BookmarkTitle>();
    public ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
}
