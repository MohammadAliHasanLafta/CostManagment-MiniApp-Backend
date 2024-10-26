using CostManagment.Application.Cost.Commands.Create;
using CostManagment.Application.Cost.Commands.Delete;
using CostManagment.Application.Cost.Commands.Update;
using CostManagment.Application.Cost.Queries.Get;
using CostManagment.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostManagment.API.Controllers;

[ApiController]
[Route("/")]
public class CostController : ControllerBase
{
    private readonly IMediator _mediator;

    public CostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-all-costs")]
    public async Task<ActionResult<IEnumerable<CostDto>>> GetCosts([FromQuery] GetCostQuery query)
    {
        var costs = await _mediator.Send(query);
        return Ok(costs);
    }

    [HttpPost("create-cost")]
    public async Task<ActionResult<long>> CreateCost(CreateCostCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update-cost/{id}")]
    public async Task<IActionResult> UpdateCost(long id, UpdateCostCommand command)
    {
        if (id != command.Id) return BadRequest();

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return Ok(result);
    }

    [HttpDelete("remove-cost/{id}")]
    public async Task<IActionResult> DeleteCost(long id)
    {
        var result = await _mediator.Send(new DeleteCostCommand { Id = id });
        if (!result) return NotFound();

        return Ok(result);
    }
}
