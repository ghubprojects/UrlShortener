using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Aggregates.ShortUrls;

namespace UrlShortener.Infrastructure.Persistence.EntityConfigurations;

public sealed class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("short_urls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ShortCode)
            .HasConversion(
                code => code.Value,
                value => ShortCode.Create(value).Value)
            .HasMaxLength(16)
            .IsRequired();

        builder.HasIndex(x => x.ShortCode)
            .IsUnique();

        builder.Property(x => x.DestinationUrl)
            .HasConversion(
                url => url.Value,
                value => Url.Create(value).Value)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(x => x.IsEnabled)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnType("timestamptz");

        builder.Ignore(x => x.DomainEvents);
    }
}
