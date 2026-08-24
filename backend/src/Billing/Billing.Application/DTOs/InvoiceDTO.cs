namespace Billing.Application.DTOs;

public record InvoiceDTO(Guid Id, int Number, string Status, List<InvoiceItemDTO> Items, DateTime CreatedAt, DateTime? UpdatedAt);
