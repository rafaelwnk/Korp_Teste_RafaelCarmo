using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Extensions;

public static class ProductMappingExtensions
{
    public static ProductDTO ToDto(this Product product)
        => new(
            product.Id,
            product.Code,
            product.Description,
            product.StockBalance,
            product.CreatedAt,
            product.UpdatedAt
        );

    public static List<ProductDTO> ToDto(this IReadOnlyList<Product> products)
        => [.. products.Select(p => p.ToDto())];

    public static Product ToEntity(this CreateProductDTO dto)
        => new(dto.Code, dto.Description, dto.StockBalance);
}