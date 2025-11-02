namespace ImdbClone.Api.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

public class SearchHistoryListDto
{
    [Column("search_date")]
    public DateTime SearchDate { get; set; }

    [Column("search_parameters")]
    public string SearchParameters { get; set; } = string.Empty;
}