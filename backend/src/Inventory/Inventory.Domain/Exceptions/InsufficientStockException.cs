namespace Inventory.Domain.Exceptions;

public class InsufficientStockException(string code, int balance, int quantity) : DomainException($"Insufficient stock for product '{code}'. Available: {balance}, requested: {quantity}.")
{
}