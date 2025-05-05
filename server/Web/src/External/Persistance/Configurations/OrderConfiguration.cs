using Domain.OrderAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new OrderId(v));

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.UserId);

        builder.Property(e => e.Status)
            .HasConversion(
                v => v.Value,
                v => OrderStatus.FromValue(v)!)
            .IsRequired();

        builder.HasMany(e => e.OrderItems)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId);

        builder.ComplexProperty(e => e.Address, b =>
        {
            b.Property(a => a.Street)
                .HasMaxLength(Address.StreetMaxLength)
                .IsRequired();

            b.Property(a => a.Building)
                .HasMaxLength(Address.BuildingMaxLength)
                .IsRequired();

            b.Property(a => a.Room)
                .HasMaxLength(Address.RoomMaxLength)
                .IsRequired(false);

            b.Property(a => a.Code)
                .HasMaxLength(Address.CodeLength)
                .IsRequired()
                .IsUnicode(false);

            b.Property(a => a.Post)
                .HasMaxLength(Address.PostMaxLength)
                .IsRequired();
        });
    }
}