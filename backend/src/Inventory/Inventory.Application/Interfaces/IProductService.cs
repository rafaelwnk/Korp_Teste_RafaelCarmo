using Inventory.Application.Common;
using Inventory.Application.DTOs;

namespace Inventory.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDTO>> GetAsync(int page = 1, int pageSize = 10);
    Task<Result<ProductDTO>> GetByIdAsync(Guid id);
    Task<Result<ProductDTO>> CreateAsync(CreateProductDTO dto);
    Task<Result<ProductDTO>> IncreaseStockAsync(Guid id, int quantity);
    Task<Result<ProductDTO>> DecreaseStockAsync(Guid id, int quantity);
    Task<Result<ProductDTO>> UpdateDescriptionAsync(Guid id, string description);
    Task<Result<bool>> DeleteAsync(Guid id);
}