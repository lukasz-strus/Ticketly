namespace Domain.Core.Primitives;

public abstract class Entity<TId> where TId : notnull
{
    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public TId Id { get; protected set; } = default!;

    protected bool Equals(Entity<TId> other) =>
        EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((Entity<TId>)obj);
    }

    public override int GetHashCode() =>
        EqualityComparer<TId>.Default.GetHashCode(Id) * 48;

    public static bool operator ==(Entity<TId> left, Entity<TId> right) =>
        Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) =>
        !Equals(left, right);
}