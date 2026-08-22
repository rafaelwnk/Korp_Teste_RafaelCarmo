namespace Inventory.Application.DTOs;

public record ProductDTO(Guid Id, string Code, string Description, int StockBalance, DateTime CreatedAt, DateTime? UpdatedAt);
public record CreateProductDTO(string Code, string Description, int StockBalance);
public record AdjustStockDto(int Quantity);
public record UpdateDescriptionDto(string Description);