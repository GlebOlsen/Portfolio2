namespace IMDB.DataService.Db.Entities;

public class Profession
{
    public Guid ProfessionId { get; set; }
    public string ProfessionName { get; set; } = string.Empty;
    public ICollection<Person> People { get; set; } = new List<Person>();
}