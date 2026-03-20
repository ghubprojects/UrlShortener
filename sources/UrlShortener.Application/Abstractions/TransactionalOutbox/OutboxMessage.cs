namespace UrlShortener.Application.Abstractions.TransactionalOutbox;

public sealed record OutboxMessage
{
    public Guid Id { get; }
    public DateTime CreationDate { get; }
    public string Payload { get; } = default!;
    public string PayloadType { get; } = default!;

    public DateTime? ProcessedDate { get; set; } // null if not processed, actually we should delete the record after processing but we keep it for now for simplicity
    public int ProcessedCount { get; set; }

    public OutboxMessage(string payload, string payloadType)
    {
        Id = Guid.CreateVersion7();
        CreationDate = DateTime.UtcNow;
        Payload = payload;
        PayloadType = payloadType;
    }
}