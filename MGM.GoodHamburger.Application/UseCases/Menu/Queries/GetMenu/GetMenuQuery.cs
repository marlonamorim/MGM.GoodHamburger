using MediatR;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Application.UseCases.Menu.Queries.GetMenu;

public record GetMenuQuery : IRequest<List<MenuItemDto>>;