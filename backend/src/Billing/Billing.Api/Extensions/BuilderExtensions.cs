using Billing.Application.Interfaces;
using Billing.Application.Services;
using Billing.Infrastructure;

namespace Billing.Api.Extensions;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddScoped<IInvoiceService, InvoiceService>();
    }
}
