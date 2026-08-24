using System.Net.Http.Json;
using Billing.Infrastructure.Common;
using Billing.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Billing.Infrastructure.ExternalServices;

public class InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger) : IInventoryServiceClient
{
    public async Task<InventoryResult> DecreaseStockAsync(Guid productId, int quantity, CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync(
                $"products/{productId}/decrease",
                new { Quantity = quantity },
                ct);

            if (response.IsSuccessStatusCode)
                return InventoryResult.Ok();

            var errorBody = await response.Content.ReadAsStringAsync(ct);

            logger.LogWarning(
                "Inventory service rejected stock decrease for product {ProductId}: {StatusCode} - {Body}",
                productId, response.StatusCode, errorBody);

            return InventoryResult.Fail(ExtractErrorMessage(errorBody) ?? "The Inventory service rejected the request.");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogError(ex, "Failed to reach the Inventory service to decrease stock for product {ProductId}.", productId);
            return InventoryResult.Fail("The invoice could not be closed because the Inventory service is unavailable. Please try again.");
        }
    }

    private static string? ExtractErrorMessage(string body)
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(body);
            return doc.RootElement.TryGetProperty("message", out var msg) ? msg.GetString() : null;
        }
        catch
        {
            return null;
        }
    }
}
