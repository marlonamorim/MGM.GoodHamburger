using MediatR;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    Guid OrderId,
    int? SandwichId,
    int? SideDishId,
    int? DrinkId
) : IRequest<OrderDto>;