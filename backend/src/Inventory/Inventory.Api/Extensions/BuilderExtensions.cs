using Inventory.Application.Interfaces;
using Inventory.Application.Services;
using Inventory.Infrastructure;

namespace Inventory.Api.Extensions;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddScoped<IProductService, ProductService>();
    }
}