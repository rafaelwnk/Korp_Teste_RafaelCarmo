namespace Billing.Infrastructure.Common;

public record InventoryResult(bool Success, string? Message)
{
    public static InventoryResult Ok() => new(true, null);
    public static InventoryResult Fail(string message) => new(false, message);
}