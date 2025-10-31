namespace ImdbClone.Api.Services;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);
    public string? NextPage { get; set; }
    public string? PreviousPage { get; set; }
};

public class PaginationService(IHttpContextAccessor _httpContextAccessor)
{
    public void SetPaginationUrls<T>(
        PaginatedResult<T> result,
        string currentPath,
        Dictionary<string, string?> queryParams
    )
    {
        var baseUrl =
            $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{currentPath}";

        var queryParts = new List<string>();

        foreach (var param in queryParams)
        {
            if (param.Key != "page" && !string.IsNullOrEmpty(param.Value))
            {
                queryParts.Add($"{param.Key}={Uri.EscapeDataString(param.Value)}");
            }
        }

        var baseQuery = string.Join("&", queryParts);
        if (result.Page > 0)
        {
            var prevQuery = string.IsNullOrEmpty(baseQuery)
                ? $"page={result.Page - 1}&pageSize={result.PageSize}"
                : $"{baseQuery}&page={result.Page - 1}&pageSize={result.PageSize}";
            result.PreviousPage = $"{baseUrl}?{prevQuery}";
        }

        if (result.Page < result.TotalPages - 1)
        {
            var nextQuery = string.IsNullOrEmpty(baseQuery)
                ? $"page={result.Page + 1}&pageSize={result.PageSize}"
                : $"{baseQuery}&page={result.Page + 1}&pageSize={result.PageSize}";
            result.NextPage = $"{baseUrl}?{nextQuery}";
        }
    }
}
