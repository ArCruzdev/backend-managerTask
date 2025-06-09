using Application.Features.Users.Command;
using Application.Features.Users.Command.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();
        return Ok(user);
    }

    // GET: api/users/role/{role}
    [HttpGet("role/{role}")]
    public async Task<ActionResult<List<UserDto>>> GetUsersByRole(string role)
    {
        var users = await _mediator.Send(new GetUsersByRoleQuery(role));
        return Ok(users);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id }, null);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return result ? NoContent() : NotFound();
    }
}


