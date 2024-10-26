
using CostManagment.Application.Income.Commands.Create;
using CostManagment.Application.Income.Queries.Get;
using CostManagment.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostManagment.API.Controllers;

[ApiController]
[Route("/")]
public class IncomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public IncomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-income")]
    public async Task<ActionResult<IEnumerable<CostDto>>> GetIncome([FromQuery] GetIncomeQuery query)
    {
        var income = await _mediator.Send(query);
        return Ok(income);
    }

    [HttpPost("create-income")]
    public async Task<ActionResult<long>> CreateCost(CreateIncomeCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
