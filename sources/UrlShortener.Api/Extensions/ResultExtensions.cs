using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Shared.Results;

namespace UrlShortener.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(
        this Result<T> result,
        Func<T, IResult>? onSuccess = null)
    {
        if (result.IsSuccess)
        {
            return onSuccess != null
                ? onSuccess(result.Value)
                : TypedResults.Ok(result.Value);
        }

        var problem = CreateProblem(result.Error);

        return result.Error.Type switch
        {
            ErrorType.Validation => TypedResults.BadRequest(problem),
            ErrorType.Unauthorized => TypedResults.Unauthorized(),
            ErrorType.Forbidden => TypedResults.Forbid(),
            ErrorType.NotFound => TypedResults.NotFound(problem),
            ErrorType.Conflict => TypedResults.Conflict(problem),
            _ => TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail)
        };
    }

    private static ProblemDetails CreateProblem(Error error)
    {
        var problem = new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message
        };

        problem.Extensions["traceId"] = Activity.Current?.Id;

        return problem;
    }
}
