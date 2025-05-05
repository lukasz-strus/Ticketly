using Domain.EventAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                v => v.Value,
                v => new EventId(v));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasConversion(
                v => v.Value,
                v => Name.Create(v).Value())
            .HasMaxLength(Name.MaxLength)
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasConversion(
                v => v.Value,
                v => Description.Create(v).Value())
            .IsRequired();

        builder.ComplexProperty(e => e.Location, b =>
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

        builder.HasMany(e => e.TicketPools)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId);
    }
}