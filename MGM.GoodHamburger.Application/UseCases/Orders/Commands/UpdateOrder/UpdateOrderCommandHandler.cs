using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Application.Exceptions;
using MGM.GoodHamburger.Domain.Entities;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Domain.Services;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            throw new NotFoundException("Pedido não encontrado.");
        }

        if (!request.SandwichId.HasValue && !request.SideDishId.HasValue && !request.DrinkId.HasValue)
        {
            throw new ValidationException("O pedido deve conter pelo menos um item.");
        }

        MenuItem? sandwich = null;
        MenuItem? sideDish = null;
        MenuItem? drink = null;
        decimal subtotal = 0;

        if (request.SandwichId.HasValue)
        {
            sandwich = await _menuItemRepository.GetByIdAsync(request.SandwichId.Value, cancellationToken);
            if (sandwich == null || sandwich.Type != MenuItemType.Sandwich)
            {
                throw new ValidationException("Sanduíche inválido.");
            }
            subtotal += sandwich.Price;
        }

        if (request.SideDishId.HasValue)
        {
            sideDish = await _menuItemRepository.GetByIdAsync(request.SideDishId.Value, cancellationToken);
            if (sideDish == null || sideDish.Type != MenuItemType.SideDish)
            {
                throw new ValidationException("Acompanhamento inválido.");
            }
            subtotal += sideDish.Price;
        }

        if (request.DrinkId.HasValue)
        {
            drink = await _menuItemRepository.GetByIdAsync(request.DrinkId.Value, cancellationToken);
            if (drink == null || drink.Type != MenuItemType.Drink)
            {
                throw new ValidationException("Bebida inválida.");
            }
            subtotal += drink.Price;
        }

        var discountPercentage = DiscountCalculator.CalculateDiscount(
            sandwich != null,
            sideDish != null,
            drink != null
        );

        var discountAmount = subtotal * discountPercentage;
        var total = subtotal - discountAmount;

        order.SandwichId = request.SandwichId;
        order.Sandwich = sandwich;
        order.SideDishId = request.SideDishId;
        order.SideDish = sideDish;
        order.DrinkId = request.DrinkId;
        order.Drink = drink;
        order.Subtotal = subtotal;
        order.DiscountPercentage = discountPercentage * 100;
        order.DiscountAmount = discountAmount;
        order.Total = total;
        order.UpdatedAt = DateTime.UtcNow;

        var updatedOrder = await _orderRepository.UpdateAsync(order, cancellationToken);

        return MapToDto(updatedOrder);
    }

    private static OrderDto MapToDto(Order order)
    {
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