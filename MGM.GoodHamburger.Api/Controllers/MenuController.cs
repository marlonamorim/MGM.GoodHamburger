using MediatR;
using MGM.GoodHamburger.Application.DTOs;
using MGM.GoodHamburger.Application.UseCases.Menu.Queries.GetMenu;
using Microsoft.AspNetCore.Mvc;

namespace MGM.GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MenuController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Obtém todos os menus disponíveis
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetMenuQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}