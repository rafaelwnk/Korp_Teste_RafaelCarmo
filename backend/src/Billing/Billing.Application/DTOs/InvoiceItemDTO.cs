namespace Billing.Application.DTOs;

public record InvoiceItemDTO(Guid Id, Guid ProductId, string ProductCode, int Quantity, DateTime CreatedAt, DateTime? UpdatedAt);
public record AddInvoiceItemDTO(Guid ProductId, string ProductCode, int Quantity);