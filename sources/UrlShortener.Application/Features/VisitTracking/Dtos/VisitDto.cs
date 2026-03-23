namespace UrlShortener.Application.Features.VisitTracking.Dtos;

public sealed record VisitDto(
    Guid VisitId,
    string ShortCode,
    DateTime VisitedAt,
    string UserAgent
);