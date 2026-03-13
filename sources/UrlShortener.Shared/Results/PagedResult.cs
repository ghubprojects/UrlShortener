namespace UrlShortener.Shared.Results;

public class PagedResult<T> where T : class
{
    public int CurrentPage { get; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; }
    public IReadOnlyList<T> Items { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public PagedResult()
    {
        CurrentPage = 0;
        TotalItems = 0;
        TotalPages = 0;
        Items = [];
    }

    public PagedResult(IReadOnlyList<T> items, int total, int pageIndex, int pageSize)
    {
        CurrentPage = pageIndex;
        TotalItems = total;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
        Items = items ?? throw new ArgumentNullException(nameof(items), "Items cannot be null");
    }
}