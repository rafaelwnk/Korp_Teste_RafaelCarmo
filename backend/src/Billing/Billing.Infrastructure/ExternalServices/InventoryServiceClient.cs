using System.Net.Http.Json;
using Billing.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Billing.Infrastructure.ExternalServices;

public class InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger) : IInventoryServiceClient
{
    public async Task<bool> DecreaseStockAsync(Guid productId, int quantity, CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync(
                $"products/{productId}/decrease",
                new { Quantity = quantity },
                ct);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogError(ex, "Failed to reach the Inventory service to decrease stock for product {ProductId}.", productId);
            return false;
        }
    }
}
