namespace Billing.Infrastructure.Interfaces;

public interface IInventoryServiceClient
{
    Task<bool> DecreaseStockAsync(Guid productId, int quantity, CancellationToken ct = default);
}