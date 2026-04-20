using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Application.Exceptions;
using MGM.GoodHamburger.Domain.Entities;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Domain.Services;

namespace MGM.GoodHamburger.Application.UseCases.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Validar que pelo menos um item foi selecionado
        if (!request.SandwichId.HasValue && !request.SideDishId.HasValue && !request.DrinkId.HasValue)
        {
            throw new ValidationException("O pedido deve conter pelo menos um item.");
        }

        MenuItem? sandwich = null;
        MenuItem? sideDish = null;
        MenuItem? drink = null;
        decimal subtotal = 0;

        // Validar e carregar sanduíche
        if (request.SandwichId.HasValue)
        {
            sandwich = await _menuItemRepository.GetByIdAsync(request.SandwichId.Value, cancellationToken);
            if (sandwich == null || sandwich.Type != MenuItemType.Sandwich)
            {
                throw new ValidationException("Sanduíche inválido.");
            }
            subtotal += sandwich.Price;
        }

        // Validar e carregar acompanhamento
        if (request.SideDishId.HasValue)
        {
            sideDish = await _menuItemRepository.GetByIdAsync(request.SideDishId.Value, cancellationToken);
            if (sideDish == null || sideDish.Type != MenuItemType.SideDish)
            {
                throw new ValidationException("Acompanhamento inválido.");
            }
            subtotal += sideDish.Price;
        }

        // Validar e carregar bebida
        if (request.DrinkId.HasValue)
        {
            drink = await _menuItemRepository.GetByIdAsync(request.DrinkId.Value, cancellationToken);
            if (drink == null || drink.Type != MenuItemType.Drink)
            {
                throw new ValidationException("Bebida inválida.");
            }
            subtotal += drink.Price;
        }

        // Calcular desconto
        var discountPercentage = DiscountCalculator.CalculateDiscount(
            sandwich != null,
            sideDish != null,
            drink != null
        );

        var discountAmount = subtotal * discountPercentage;
        var total = subtotal - discountAmount;

        // Criar pedido
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            SandwichId = request.SandwichId,
            Sandwich = sandwich,
            SideDishId = request.SideDishId,
            SideDish = sideDish,
            DrinkId = request.DrinkId,
            Drink = drink,
            Subtotal = subtotal,
            DiscountPercentage = discountPercentage * 100,
            DiscountAmount = discountAmount,
            Total = total
        };

        var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);

        return MapToDto(createdOrder);
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