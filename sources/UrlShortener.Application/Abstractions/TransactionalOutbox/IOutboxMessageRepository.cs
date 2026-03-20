namespace UrlShortener.Application.Abstractions.TransactionalOutbox;

public interface IOutboxMessageRepository
{
    Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(
        CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(OutboxMessage message);

    Task MarkAsFailedAsync(OutboxMessage message, bool recoverable = true);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
