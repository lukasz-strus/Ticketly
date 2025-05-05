using Domain.EventAggregate;
using Domain.OrderAggregate;
using Domain.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Domain.ValueObjects;

namespace Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Owned<FirstName>();
        builder.Owned<LastName>();
        builder.Owned<Address>();
        builder.Owned<Description>();
        builder.Owned<Name>();

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<TicketPool> TicketPools { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
