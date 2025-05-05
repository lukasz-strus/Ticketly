using Domain.EventAggregate;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.EventAggregate;

public sealed class CategoryTests
{
    [Fact]
    public void Create_ShouldReturnSuccessResultWithCategory()
    {
        // Arrange
        var name = Name.Create("Category Name").Value();

        // Act
        var result = Category.Create(name);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var category = result.Value();
        category.Should().NotBeNull();
        category.Name.Should().Be(name);
    }

}
