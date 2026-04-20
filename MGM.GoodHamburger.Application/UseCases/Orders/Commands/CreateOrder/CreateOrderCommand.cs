using MediatR;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    int? SandwichId,
    int? SideDishId,
    int? DrinkId
) : IRequest<OrderDto>;