using MediatR;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) : IRequest<bool>;