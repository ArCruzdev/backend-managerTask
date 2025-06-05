using Application.Features.TaskItems.Commands.CreateTaskItem;
using Application.Features.TaskItems.Commands.DeleteTaskItem;
using Application.Features.TaskItems.Commands.UpdateTaskItem;
using Application.Features.TaskItems.Queries;
using Application.Features.TaskItems.Queries.GetTasksByProjectId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTaskItemsQuery());
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTaskItemByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskItemCommand command)
        {
            if (id != command.Id) return BadRequest();
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTaskItemCommand(id));
            return result ? NoContent() : NotFound();
        }

        [HttpGet("project/{projectId}")] // La ruta que tu frontend ya está llamando
        public async Task<ActionResult<List<TaskItemDto>>> GetTasksByProjectId(string projectId)
        {
            var query = new GetTasksByProjectIdQuery(projectId);
            try
            {
                var tasks = await _mediator.Send(query);

                if (tasks == null || tasks.Count == 0)
                {
                    return NotFound($"No se encontraron tareas para el proyecto con ID: {projectId}");
                }

                return Ok(tasks);
            }
            catch (ArgumentException ex) // Captura la excepción si el GUID no es válido
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

