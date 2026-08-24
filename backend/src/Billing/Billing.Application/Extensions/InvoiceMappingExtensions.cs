using Billing.Application.DTOs;
using Billing.Domain.Entities;

namespace Billing.Application.Extensions;

public static class InvoiceMappingExtensions
{
    public static InvoiceItemDTO ToDto(this InvoiceItem item)
        => new(item.Id, item.ProductId, item.ProductCode, item.Quantity, item.CreatedAt, item.UpdatedAt);

    public static InvoiceDTO ToDto(this Invoice invoice)
        => new(
            invoice.Id,
            invoice.Number,
            invoice.Status.ToString(),
            [.. invoice.Items.Select(i => i.ToDto())],
            invoice.CreatedAt,
            invoice.UpdatedAt
        );

    public static List<InvoiceDTO> ToDto(this IReadOnlyList<Invoice> invoices)
        => [.. invoices.Select(i => i.ToDto())];
}
