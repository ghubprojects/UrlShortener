using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UrlShortener.Application.Abstractions.TransactionalOutbox;
using UrlShortener.Domain.Aggregates.ShortUrls;
using UrlShortener.Domain.Entities.Visits;
using UrlShortener.Domain.SeedWork;

namespace UrlShortener.Infrastructure.Persistence.DataContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<ShortUrl> ShortUrls { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
