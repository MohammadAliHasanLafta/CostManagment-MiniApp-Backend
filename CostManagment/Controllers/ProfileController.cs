using CostManagment.Application.Profile.Commands.Delete;
using CostManagment.Application.Profile.Commands.Update;
using CostManagment.Application.Profile.Queries.Get;
using CostManagment.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CostManagment.API.Controllers;

[ApiController]
[Route("/")]
public class ProfileContorller : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileContorller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-profile")]
    public async Task<ActionResult<UserProfile>> GetProfile([FromQuery] GetProfileQuery query)
    {
        var profile = await _mediator.Send(query);
        return Ok(profile);
    }

    [HttpPut("update-profile/{id}")]
    public async Task<IActionResult> UpdateTodo(long id, UpdateProfileCommand command)
    {
        if (id != command.Id) return BadRequest();

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return Ok(result);
    }

    [HttpDelete("remove-profile/{id}")]
    public async Task<IActionResult> DeleteTodo(long id)
    {
        var result = await _mediator.Send(new DeleteProfileCommand { Id = id });
        if (!result) return NotFound();

        return Ok(result);
    }
}
