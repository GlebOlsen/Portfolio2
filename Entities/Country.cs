using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Country
{
    [Key]
    [Column("country_id")]
    public Guid CountryId { get; set; }

    [Column("country_name")]
    public string CountryName { get; set; } = string.Empty;

    public ICollection<Title> Titles { get; set; } = new List<Title>();
}
