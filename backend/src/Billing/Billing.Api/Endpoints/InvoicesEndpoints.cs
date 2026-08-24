using Billing.Api.Extensions;
using Billing.Application.DTOs;
using Billing.Application.Interfaces;

namespace Billing.Api.Endpoints;

public static class InvoicesEndpoints
{
    public static void MapInvoicesEndpoints(this WebApplication app)
    {
        var invoices = app.MapGroup("invoices");

        invoices.MapGet("", async (IInvoiceService invoiceService, int page = 1, int pageSize = 10)
            => (await invoiceService.GetAsync(page, pageSize)).ToOkOrBadRequestResult());

        invoices.MapGet("{id:guid}", async (IInvoiceService invoiceService, Guid id)
            => (await invoiceService.GetByIdAsync(id)).ToOkOrNotFoundResult());

        invoices.MapPost("", async (IInvoiceService invoiceService) =>
        {
            var result = await invoiceService.CreateAsync();
            var uri = $"/invoices/{result.Data?.Id}";
            return result.ToCreatedOrBadRequestResult(uri);
        });

        invoices.MapPost("{id:guid}/items", async (IInvoiceService invoiceService, Guid id, AddInvoiceItemDTO dto)
            => (await invoiceService.AddItemAsync(id, dto)).ToOkOrBadRequestResult());

        invoices.MapDelete("{id:guid}/items/{itemId:guid}", async (IInvoiceService invoiceService, Guid id, Guid itemId)
            => (await invoiceService.RemoveItemAsync(id, itemId)).ToOkOrBadRequestResult());

        invoices.MapPatch("{id:guid}/close", async (IInvoiceService invoiceService, Guid id)
            => (await invoiceService.CloseAsync(id)).ToOkOrBadRequestResult());

        invoices.MapDelete("{id:guid}", async (IInvoiceService invoiceService, Guid id)
            => (await invoiceService.DeleteAsync(id)).ToNoContentOrBadRequestResult());
    }
}
