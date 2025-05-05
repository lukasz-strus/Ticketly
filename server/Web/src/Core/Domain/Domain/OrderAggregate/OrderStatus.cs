using Domain.Core.Primitives;

namespace Domain.OrderAggregate;

public sealed class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(1, nameof(Pending));
    public static readonly OrderStatus Completed = new(2, nameof(Completed));
    public static readonly OrderStatus Cancelled = new(3, nameof(Cancelled));

    private OrderStatus(int value, string name) : base(value, name)
    {
    }

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    private OrderStatus()
    {
        
    }
}