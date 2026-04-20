using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Application.Exceptions;
using MGM.GoodHamburger.Domain.Repositories;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        
        if (order == null)
        {
            throw new NotFoundException("Pedido não encontrado.");
        }

        return new OrderDto(
            order.Id,
            order.CreatedAt,
            order.UpdatedAt,
            order.Sandwich != null ? new MenuItemDto(order.Sandwich.Id, order.Sandwich.Name, order.Sandwich.Price, order.Sandwich.Type.ToString()) : null,
            order.SideDish != null ? new MenuItemDto(order.SideDish.Id, order.SideDish.Name, order.SideDish.Price, order.SideDish.Type.ToString()) : null,
            order.Drink != null ? new MenuItemDto(order.Drink.Id, order.Drink.Name, order.Drink.Price, order.Drink.Type.ToString()) : null,
            order.Subtotal,
            order.DiscountPercentage,
            order.DiscountAmount,
            order.Total
        );
    }
}