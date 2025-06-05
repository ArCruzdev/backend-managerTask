using MediatR;
using System.Collections.Generic;

namespace Application.Features.TaskItems.Queries;

public record GetAllTaskItemsQuery : IRequest<List<TaskItemDto>>;

