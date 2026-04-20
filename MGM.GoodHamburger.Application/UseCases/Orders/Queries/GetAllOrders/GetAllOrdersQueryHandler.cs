using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Domain.Repositories;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        
        return orders.Select(order => new OrderDto(
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
        )).ToList();
    }
}