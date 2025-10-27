namespace ImdbClone.Api.DTOs.Users;

public class TitleRatingListDto
{
    public string Tconst { get; set; }
    public string PrimaryTitle { get; set; }
    public int Rating { get; set; }
    public DateTime RatingDate { get; set; }
}