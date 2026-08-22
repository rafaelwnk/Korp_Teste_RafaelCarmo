using Microsoft.EntityFrameworkCore;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Persistence;
using Inventory.Application.Common;
using Inventory.Application.Extensions;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public async Task<PagedResult<ProductDTO>> GetAsync(int page = 1, int pageSize = 10)
    {
        var totalItems = await context.Products.CountAsync();

        var items = await context.Products
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PagedResult<ProductDTO>.Success(items.ToDto(), page, pageSize, totalItems);
    }
    public async Task<Result<ProductDTO>> GetByIdAsync(Guid id)
    {
        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return Result<ProductDTO>.Error($"Product with id '{id}' was not found.");

        return Result<ProductDTO>.Success(product.ToDto());
    }
    public async Task<Result<ProductDTO>> CreateAsync(CreateProductDTO dto)
    {
        var exists = await context.Products.AnyAsync(x => x.Code == dto.Code);

        if (exists) return Result<ProductDTO>.Error($"A product with code '{dto.Code}' already exists.");

        var creationResult = ResultFactory.Try(() => dto.ToEntity());

        if (!string.IsNullOrWhiteSpace(creationResult.Message))
            return Result<ProductDTO>.Error(creationResult.Message);

        var product = creationResult.Data!;

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Result<ProductDTO>.Success(product.ToDto());
    }

    public Task<Result<ProductDTO>> IncreaseStockAsync(Guid id, AdjustStockDto dto)
        => ExecuteAsync(id, p => p.IncreaseStockBalance(dto.Quantity));

    public Task<Result<ProductDTO>> DecreaseStockAsync(Guid id, AdjustStockDto dto)
        => ExecuteAsync(id, p => p.DecreaseStockBalance(dto.Quantity));

    public Task<Result<ProductDTO>> UpdateDescriptionAsync(Guid id, UpdateDescriptionDto dto)
        => ExecuteAsync(id, p => p.UpdateDescription(dto.Description));

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product is null)
            return Result<bool>.Error($"Product with id '{id}' was not found.");

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    private async Task<Result<ProductDTO>> ExecuteAsync(Guid id, Action<Product> action)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product is null)
            return Result<ProductDTO>.Error($"Product with id '{id}' was not found.");

        var result = ResultFactory.Try(() =>
        {
            action(product);
            return product;
        });

        if (!string.IsNullOrWhiteSpace(result.Message))
            return Result<ProductDTO>.Error(result.Message);

        await context.SaveChangesAsync();
        return Result<ProductDTO>.Success(result.Data!.ToDto());
    }
}