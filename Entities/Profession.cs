using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbClone.Api.Entities;

public class Profession
{
    [Key]
    [Column("profession_id")]
    public Guid ProfessionId { get; set; }

    [Column("profession_name")]
    public string ProfessionName { get; set; } = string.Empty;
    public ICollection<Person> People { get; set; } = new List<Person>();
}
