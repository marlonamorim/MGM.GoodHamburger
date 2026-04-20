using FluentAssertions;
using MGM.GoodHamburger.Application.UseCases.Orders.Commands.CreateOrder;
using MGM.GoodHamburger.IntegrationTests.Infrastructure;

namespace MGM.GoodHamburger.IntegrationTests.UseCases.Orders;

public class CreateOrderCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateOrder_WithSandwichAndSideDish_ShouldApply10PercentDiscount()
    {
        // Arrange
        var command = new CreateOrderCommand(
            SandwichId: 1,    // X Burger - R$ 5.00
            SideDishId: 4,    // Batata frita - R$ 2.00
            DrinkId: null
        );

        var expectedSubtotal = 7.00m;         // 5.00 + 2.00
        var expectedDiscountPercentage = 10m; // 10%
        var expectedDiscountAmount = 0.70m;   // 7.00 * 0.10
        var expectedTotal = 6.30m;            // 7.00 - 0.70

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Sandwich.Should().NotBeNull();
        result.Sandwich!.Id.Should().Be(1);
        result.Sandwich.Name.Should().Be("X Burger");
        result.SideDish.Should().NotBeNull();
        result.SideDish!.Id.Should().Be(4);
        result.SideDish.Name.Should().Be("Batata frita");
        result.Drink.Should().BeNull();
        
        result.Subtotal.Should().Be(expectedSubtotal);
        result.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        result.DiscountAmount.Should().Be(expectedDiscountAmount);
        result.Total.Should().Be(expectedTotal);

        // Verifica se foi salvo no banco
        var orderInDb = await DbContext.Orders.FindAsync(result.Id);
        orderInDb.Should().NotBeNull();
        orderInDb!.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichAndDrink_ShouldApply15PercentDiscount()
    {
        // Arrange
        var command = new CreateOrderCommand(
            SandwichId: 2,    // X Egg - R$ 4.50
            SideDishId: null,
            DrinkId: 5        // Refrigerante - R$ 2.50
        );

        var expectedSubtotal = 7.00m;         // 4.50 + 2.50
        var expectedDiscountPercentage = 15m; // 15%
        var expectedDiscountAmount = 1.05m;   // 7.00 * 0.15
        var expectedTotal = 5.95m;            // 7.00 - 1.05

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Sandwich.Should().NotBeNull();
        result.Sandwich!.Id.Should().Be(2);
        result.Sandwich.Name.Should().Be("X Egg");
        result.SideDish.Should().BeNull();
        result.Drink.Should().NotBeNull();
        result.Drink!.Id.Should().Be(5);
        result.Drink.Name.Should().Be("Refrigerante");
        
        result.Subtotal.Should().Be(expectedSubtotal);
        result.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        result.DiscountAmount.Should().Be(expectedDiscountAmount);
        result.Total.Should().Be(expectedTotal);

        // Verifica se foi salvo no banco
        var orderInDb = await DbContext.Orders.FindAsync(result.Id);
        orderInDb.Should().NotBeNull();
        orderInDb!.Total.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_WithSandwichSideDishAndDrink_ShouldApply20PercentDiscount()
    {
        // Arrange
        var command = new CreateOrderCommand(
            SandwichId: 3,    // X Bacon - R$ 7.00
            SideDishId: 4,    // Batata frita - R$ 2.00
            DrinkId: 5        // Refrigerante - R$ 2.50
        );

        var expectedSubtotal = 11.50m;        // 7.00 + 2.00 + 2.50
        var expectedDiscountPercentage = 20m; // 20%
        var expectedDiscountAmount = 2.30m;   // 11.50 * 0.20
        var expectedTotal = 9.20m;            // 11.50 - 2.30

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Sandwich.Should().NotBeNull();
        result.Sandwich!.Id.Should().Be(3);
        result.Sandwich.Name.Should().Be("X Bacon");
        result.SideDish.Should().NotBeNull();
        result.SideDish!.Id.Should().Be(4);
        result.SideDish.Name.Should().Be("Batata frita");
        result.Drink.Should().NotBeNull();
        result.Drink!.Id.Should().Be(5);
        result.Drink.Name.Should().Be("Refrigerante");
        
        result.Subtotal.Should().Be(expectedSubtotal);
        result.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        result.DiscountAmount.Should().Be(expectedDiscountAmount);
        result.Total.Should().Be(expectedTotal);

        // Verifica se foi salvo no banco
        var orderInDb = await DbContext.Orders.FindAsync(result.Id);
        orderInDb.Should().NotBeNull();
        orderInDb!.Total.Should().Be(expectedTotal);
        orderInDb.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        orderInDb.DiscountAmount.Should().Be(expectedDiscountAmount);
    }
}
