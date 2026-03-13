using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities.Visits;

namespace UrlShortener.Infrastructure.Persistence.EntityConfigurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("visits");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ShortUrlId)
            .IsRequired();

        builder.Property(x => x.ShortCode)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.VisitedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(512);

        builder.Property(x => x.Referer)
            .HasMaxLength(2048);

        builder.HasIndex(x => x.ShortUrlId);
    }
}