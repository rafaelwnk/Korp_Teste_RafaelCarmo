using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int StockBalance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }
    public Product(string code, string description, int stockBalance)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Product code is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description is required.");

        if (stockBalance < 0)
            throw new DomainException("Initial stock balance cannot be negative.");

        Id = Guid.NewGuid();
        Code = code;
        Description = description;
        StockBalance = stockBalance;
        CreatedAt = DateTime.UtcNow;
    }

    public void IncreaseStockBalance(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to increase must be greater than zero.");

        StockBalance += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecreaseStockBalance(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to decrease must be greater than zero.");

        if (StockBalance < quantity)
            throw new InsufficientStockException(Code, StockBalance, quantity);

        StockBalance -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description is required.");

        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}