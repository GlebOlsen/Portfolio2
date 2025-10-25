namespace ImdbClone.Api.Entities;

public class Country
{
    public Guid CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public ICollection<Title> Titles { get; set; } = new List<Title>();
}
