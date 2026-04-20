namespace MGM.GoodHamburger.Application.DTOs;

public record OrderDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    MenuItemDto? Sandwich,
    MenuItemDto? SideDish,
    MenuItemDto? Drink,
    decimal Subtotal,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    decimal Total
);

public record MenuItemDto(
    int Id,
    string Name,
    decimal Price,
    string Type
);

public record CreateOrderRequest(
    int? SandwichId,
    int? SideDishId,
    int? DrinkId
);

public record UpdateOrderRequest(
    int? SandwichId,
    int? SideDishId,
    int? DrinkId
);