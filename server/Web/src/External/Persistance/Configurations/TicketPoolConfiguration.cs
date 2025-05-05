using Domain.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class TicketPoolConfiguration : IEntityTypeConfiguration<TicketPool>
{
    public void Configure(EntityTypeBuilder<TicketPool> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new TicketPoolId(v));

        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.Price, b =>
        {
            b.WithOwner();

            b.Property(p => p.Value)
                .HasColumnName("Amount")
                .HasColumnType("decimal(19,4)")
                .IsRequired();

            b.OwnsOne(p => p.Currency, c =>
            {
                c.WithOwner();

                c.Property(v => v.Value)
                    .HasColumnName("Currency")
                    .IsRequired();

                c.Ignore(v => v.Code);

                c.Ignore(v => v.Name);

            });
        });
    }
}