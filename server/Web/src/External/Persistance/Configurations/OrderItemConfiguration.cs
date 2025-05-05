using Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new OrderItemId(v));

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.TicketPool)
            .WithMany()
            .HasForeignKey(e => e.TicketPoolId)
            .IsRequired();

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