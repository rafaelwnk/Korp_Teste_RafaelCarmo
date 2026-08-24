using Billing.Domain.Enums;
using Billing.Domain.Exceptions;

namespace Billing.Domain.Entities;

public class Invoice
{
    public Guid Id { get; private set; }
    public int Number { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private readonly List<InvoiceItem> _items = [];
    public IReadOnlyList<InvoiceItem> Items => _items.AsReadOnly();

    public Invoice()
    {
        Id = Guid.NewGuid();
        Status = InvoiceStatus.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, string productCode, int quantity)
    {
        if (Status != InvoiceStatus.Open)
            throw new InvalidInvoiceStatusException(Number, "add items");

        if (productId == Guid.Empty)
            throw new DomainException("Product id is required.");

        if (string.IsNullOrWhiteSpace(productCode))
            throw new DomainException("Product code is required.");

        if (quantity <= 0)
            throw new DomainException("Item quantity must be greater than zero.");

        var existing = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existing is not null)
            existing.IncreaseQuantity(quantity);
        else
            _items.Add(new InvoiceItem(Id, productId, productCode, quantity));

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != InvoiceStatus.Open)
            throw new InvalidInvoiceStatusException(Number, "remove items");

        var item = _items.FirstOrDefault(i => i.Id == itemId);

        if (item is null)
            throw new DomainException($"Item with id '{itemId}' was not found in this invoice.");

        _items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != InvoiceStatus.Open)
            throw new InvalidInvoiceStatusException(Number, "close");

        if (_items.Count == 0)
            throw new DomainException("Cannot close an invoice without items.");

        Status = InvoiceStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }
}