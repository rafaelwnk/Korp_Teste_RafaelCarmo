using Billing.Application.Common;
using Billing.Application.DTOs;

namespace Billing.Application.Interfaces;

public interface IInvoiceService
{
    Task<PagedResult<InvoiceDTO>> GetAsync(int page = 1, int pageSize = 10);
    Task<Result<InvoiceDTO>> GetByIdAsync(Guid id);
    Task<Result<InvoiceDTO>> CreateAsync();
    Task<Result<InvoiceDTO>> AddItemAsync(Guid id, AddInvoiceItemDTO dto);
    Task<Result<InvoiceDTO>> RemoveItemAsync(Guid id, Guid itemId);
    Task<Result<InvoiceDTO>> CloseAsync(Guid id);
    Task<Result<bool>> DeleteAsync(Guid id);
}
