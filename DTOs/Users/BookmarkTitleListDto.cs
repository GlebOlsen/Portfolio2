namespace ImdbClone.Api.DTOs.Users;

public class BookmarkTitleListDto
{
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public DateTime BookmarkDate { get; set; }
}