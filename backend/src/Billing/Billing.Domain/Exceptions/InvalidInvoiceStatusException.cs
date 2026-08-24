namespace Billing.Domain.Exceptions;

public class InvalidInvoiceStatusException(int number, string action)
    : DomainException($"Invoice '{number}' cannot {action} because its current status does not allow it.")
{
}
