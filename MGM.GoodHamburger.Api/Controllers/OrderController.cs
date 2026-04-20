using MediatR;
using Microsoft.AspNetCore.Mvc;
using MGM.GoodHamburger.Application.UseCases.Orders.Commands.CreateOrder;
using MGM.GoodHamburger.Application.UseCases.Orders.Commands.UpdateOrder;
using MGM.GoodHamburger.Application.UseCases.Orders.Commands.DeleteOrder;
using MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetAllOrders;
using MGM.GoodHamburger.Application.UseCases.Orders.Queries.GetOrderById;
using MGM.GoodHamburger.Application.DTOs;

namespace MGM.GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Obtém todos os pedidos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllOrdersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtém um pedido específico por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cria um novo pedido
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um pedido existente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.OrderId)
            return BadRequest(new { message = "O ID da rota não corresponde ao ID do corpo da requisição" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Exclui um pedido
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound(new { message = $"Pedido com ID {id} não encontrado" });

        return NoContent();
    }
}