using MediatR;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetAllOrders;

public record GetAllOrdersQuery : IRequest<List<OrderDto>>;