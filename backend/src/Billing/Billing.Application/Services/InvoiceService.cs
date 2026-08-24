using Microsoft.EntityFrameworkCore;
using Billing.Application.Common;
using Billing.Application.DTOs;
using Billing.Application.Extensions;
using Billing.Application.Interfaces;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Billing.Infrastructure.Interfaces;
using Billing.Domain.Enums;

namespace Billing.Application.Services;

public class InvoiceService(AppDbContext context, IInventoryServiceClient inventoryClient) : IInvoiceService
{
    public async Task<PagedResult<InvoiceDTO>> GetAsync(int page = 1, int pageSize = 10)
    {
        var totalItems = await context.Invoices.CountAsync();

        var items = await context.Invoices
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderBy(x => x.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PagedResult<InvoiceDTO>.Success(items.ToDto(), page, pageSize, totalItems);
    }

    public async Task<Result<InvoiceDTO>> GetByIdAsync(Guid id)
    {
        var invoice = await context.Invoices
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return Result<InvoiceDTO>.Error($"Invoice with id '{id}' was not found.");

        return Result<InvoiceDTO>.Success(invoice.ToDto());
    }

    public async Task<Result<InvoiceDTO>> CreateAsync()
    {
        var invoice = new Invoice();

        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();

        return Result<InvoiceDTO>.Success(invoice.ToDto());
    }

    public async Task<Result<InvoiceDTO>> AddItemAsync(Guid id, AddInvoiceItemDTO dto)
    {
        var invoice = await context.Invoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return Result<InvoiceDTO>.Error($"Invoice with id '{id}' was not found.");

        var result = ResultFactory.Try(() =>
        {
            invoice.AddItem(dto.ProductId, dto.ProductCode, dto.Quantity);
            return invoice;
        });

        if (!string.IsNullOrWhiteSpace(result.Message))
            return Result<InvoiceDTO>.Error(result.Message);

        await context.SaveChangesAsync();

        return Result<InvoiceDTO>.Success(result.Data!.ToDto());
    }

    public async Task<Result<InvoiceDTO>> RemoveItemAsync(Guid id, Guid itemId)
    {
        var invoice = await context.Invoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return Result<InvoiceDTO>.Error($"Invoice with id '{id}' was not found.");

        var result = ResultFactory.Try(() =>
        {
            invoice.RemoveItem(itemId);
            return invoice;
        });

        if (!string.IsNullOrWhiteSpace(result.Message))
            return Result<InvoiceDTO>.Error(result.Message);

        await context.SaveChangesAsync();

        return Result<InvoiceDTO>.Success(result.Data!.ToDto());
    }

    public async Task<Result<InvoiceDTO>> CloseAsync(Guid id)
    {
        var invoice = await context.Invoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return Result<InvoiceDTO>.Error($"Invoice with id '{id}' was not found.");

        var closeResult = ResultFactory.Try(() =>
        {
            invoice.Close();
            return invoice;
        });

        if (!string.IsNullOrWhiteSpace(closeResult.Message))
            return Result<InvoiceDTO>.Error(closeResult.Message);

        foreach (var item in invoice.Items)
        {
            var result = await inventoryClient.DecreaseStockAsync(item.ProductId, item.Quantity);

            if (!result.Success)
                return Result<InvoiceDTO>.Error(result.Message ?? "Could not close invoice due to an unexpected error.");
        }

        await context.SaveChangesAsync();

        return Result<InvoiceDTO>.Success(invoice.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var invoice = await context.Invoices
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return Result<bool>.Error($"Invoice with id '{id}' was not found.");

        if (invoice.Status != InvoiceStatus.Open)
            return Result<bool>.Error("Only open invoices can be deleted.");

        context.Invoices.Remove(invoice);
        await context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
