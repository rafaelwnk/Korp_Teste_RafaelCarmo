using Inventory.Api.Extensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;

namespace Inventory.Api.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication app)
    {
        var products = app.MapGroup("products");

        products.MapGet("", async (IProductService productService, int page = 1, int pageSize = 10)
            => (await productService.GetAsync(page, pageSize)).ToOkOrBadRequestResult());

        products.MapGet("{id:guid}", async (IProductService productService, Guid id)
            => (await productService.GetByIdAsync(id)).ToOkOrNotFoundResult());

        products.MapPost("", async (IProductService productService, CreateProductDTO dto) =>
        {
            var result = await productService.CreateAsync(dto);
            var uri = $"/products/{result.Data?.Id}";
            return result.ToCreatedOrBadRequestResult(uri);
        });

        products.MapPatch("{id:guid}/increase", async (IProductService productService, Guid id, AdjustStockDto dto)
            => (await productService.IncreaseStockAsync(id, dto)).ToOkOrBadRequestResult());

        products.MapPatch("{id:guid}/decrease", async (IProductService productService, Guid id, AdjustStockDto dto)
            => (await productService.DecreaseStockAsync(id, dto)).ToOkOrBadRequestResult());

        products.MapPatch("{id:guid}/description", async (IProductService productService, Guid id, UpdateDescriptionDto dto)
            => (await productService.UpdateDescriptionAsync(id, dto)).ToOkOrBadRequestResult());

        products.MapDelete("{id:guid}", async (IProductService productService, Guid id)
            => (await productService.DeleteAsync(id)).ToNoContentOrBadRequestResult());
    }
}