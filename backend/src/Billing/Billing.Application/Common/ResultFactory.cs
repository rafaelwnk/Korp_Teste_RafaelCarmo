using Billing.Domain.Exceptions;

namespace Billing.Application.Common;

public static class ResultFactory
{
    public static Result<T> Try<T>(Func<T> action)
    {
        try
        {
            return Result<T>.Success(action());
        }
        catch (DomainException ex)
        {
            return Result<T>.Error(ex.Message);
        }
    }
}