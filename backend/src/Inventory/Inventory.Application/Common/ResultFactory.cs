using Inventory.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Common;

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

    public static async Task<Result<T>> TryWithConcurrencyRetryAsync<T>(DbContext context, Func<Task<T>> operation, int maxRetries = 10)
    {
        var attempt = 1;

        while (true)
        {
            try
            {
                context.ChangeTracker.Clear();
                return Result<T>.Success(await operation());
            }
            catch (DbUpdateConcurrencyException) when (attempt < maxRetries)
            {
                attempt++;
                await Task.Delay(50 * attempt);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<T>.Error("Could not update the product due to concurrent modifications. Please try again.");
            }
            catch (DomainException ex)
            {
                return Result<T>.Error(ex.Message);
            }
        }
    }
}