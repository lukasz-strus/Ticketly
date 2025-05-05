using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.ValueObjects;

namespace Domain.EventAggregate;

public sealed class Category : Entity<CategoryId>
{
    public Name Name { get; private set; }

    private Category(Name name) : base(new CategoryId(Guid.NewGuid()))
    {
        Name = name;
    }

    public static Result<Category> Create(Name name) => 
        new Category(name);
}