using Billing.Application.Common;

namespace Billing.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToOkOrBadRequestResult<T>(this Result<T> result)
        => string.IsNullOrWhiteSpace(result.Message)
            ? Results.Ok(result)
            : Results.BadRequest(result);

    public static IResult ToOkOrNotFoundResult<T>(this Result<T> result)
        => string.IsNullOrWhiteSpace(result.Message)
            ? Results.Ok(result)
            : Results.NotFound(result);

    public static IResult ToCreatedOrBadRequestResult<T>(this Result<T> result, string uri)
        => string.IsNullOrWhiteSpace(result.Message)
            ? Results.Created(uri, result)
            : Results.BadRequest(result);

    public static IResult ToNoContentOrBadRequestResult<T>(this Result<T> result)
        => string.IsNullOrWhiteSpace(result.Message)
            ? Results.NoContent()
            : Results.BadRequest(result);

    public static IResult ToOkOrBadRequestResult<T>(this PagedResult<T> result)
        => string.IsNullOrWhiteSpace(result.Message)
            ? Results.Ok(result)
            : Results.BadRequest(result);
}
