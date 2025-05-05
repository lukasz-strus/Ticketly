using Domain.UserAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasConversion(
                v => v.Value,
                v => FirstName.Create(v).Value())
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasConversion(
                v => v.Value,
                v => LastName.Create(v).Value())
            .HasMaxLength(LastName.MaxLength)
            .IsRequired();

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
