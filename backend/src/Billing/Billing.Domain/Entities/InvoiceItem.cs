using Billing.Domain.Exceptions;

namespace Billing.Domain.Entities;

public class InvoiceItem
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductCode { get; private set; } = default!;
    public int Quantity { get; private set; }

    private InvoiceItem() { }

    public InvoiceItem(Guid invoiceId, Guid productId, string productCode, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product id is required.");

        if (string.IsNullOrWhiteSpace(productCode))
            throw new DomainException("Product code is required.");

        if (quantity <= 0)
            throw new DomainException("Item quantity must be greater than zero.");

        Id = Guid.NewGuid();
        InvoiceId = invoiceId;
        ProductId = productId;
        ProductCode = productCode;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Item quantity must be greater than zero.");

        Quantity += quantity;
    }
}
