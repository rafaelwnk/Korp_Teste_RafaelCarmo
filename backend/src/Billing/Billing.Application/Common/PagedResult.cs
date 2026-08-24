namespace Billing.Application.Common;

public class PagedResult<T>(List<T>? items, int page, int pageSize, int totalItems, string message = "")
{
    public List<T>? Items { get; private set; } = items;
    public int Page { get; private set; } = page;
    public int PageSize { get; private set; } = pageSize;
    public int TotalItems { get; private set; } = totalItems;
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
    public string Message { get; private set; } = message;

    public static PagedResult<T> Success(List<T> items, int page, int pageSize, int totalItems)
        => new(items, page, pageSize, totalItems);

    public static PagedResult<T> Error(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message cannot be empty.", nameof(message));

        return new(default, 0, 0, 0, message);
    }
}