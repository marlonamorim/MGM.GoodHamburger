using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Domain.Repositories;

namespace MGM.GoodHamburger.Application.UseCases.Menu.Queries.GetMenu;

public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, List<MenuItemDto>>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public GetMenuQueryHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<List<MenuItemDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        var items = await _menuItemRepository.GetAllAsync(cancellationToken);
        
        return items.Select(item => new MenuItemDto(
            item.Id,
            item.Name,
            item.Price,
            item.Type.ToString()
        )).ToList();
    }
}