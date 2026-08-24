using Billing.Infrastructure.Common;

namespace Billing.Infrastructure.Interfaces;

public interface IInventoryServiceClient
{
    Task<InventoryResult> DecreaseStockAsync(Guid productId, int quantity, CancellationToken ct = default);
}