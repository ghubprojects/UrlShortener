using MediatR;
using UrlShortener.Shared.Results;

namespace UrlShortener.Application.UseCases.GetVisits;

public record GetVisitsQuery(
    string Code,
    int Page,
    int PageSize
) : IRequest<PagedResult<GetVisitsResult>>;
