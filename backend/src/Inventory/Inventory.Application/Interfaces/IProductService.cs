using Inventory.Application.Common;
using Inventory.Application.DTOs;

namespace Inventory.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDTO>> GetAsync(int page = 1, int pageSize = 10);
    Task<Result<ProductDTO>> GetByIdAsync(Guid id);
    Task<Result<ProductDTO>> CreateAsync(CreateProductDTO dto);
    Task<Result<ProductDTO>> IncreaseStockAsync(Guid id, AdjustStockDto dto);
    Task<Result<ProductDTO>> DecreaseStockAsync(Guid id, AdjustStockDto dto);
    Task<Result<ProductDTO>> UpdateDescriptionAsync(Guid id, UpdateDescriptionDto dto);
    Task<Result<bool>> DeleteAsync(Guid id);
}