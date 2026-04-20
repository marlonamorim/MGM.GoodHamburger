using MediatR;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;