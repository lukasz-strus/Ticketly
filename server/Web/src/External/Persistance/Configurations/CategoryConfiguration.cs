using Domain.EventAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new CategoryId(v));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasConversion(
                v => v.Value,
                v => Name.Create(v).Value())
            .HasMaxLength(Name.MaxLength)
            .IsRequired();
    }
}