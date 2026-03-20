using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Infrastructure.Persistence.DataContext;

namespace UrlShortener.Infrastructure.TransactionalOutbox;

public class OutboxMessageRepository(AppDbContext context) : IOutboxMessageRepository
{
    private const int MaxRetries = 3;

    public async Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        await context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.OutboxMessages
            .Where(m => m.ProcessedDate == null && m.ProcessedCount < MaxRetries)
            .ToListAsync(cancellationToken);
    }

    public Task MarkAsFailedAsync(OutboxMessage message, bool recoverable = true)
    {
        if (recoverable)
            message.ProcessedCount++;
        else
            message.ProcessedCount = MaxRetries;

        return Task.CompletedTask;
    }

    public Task MarkAsProcessedAsync(OutboxMessage message)
    {
        message.ProcessedCount++;
        message.ProcessedDate = DateTime.UtcNow;

        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
