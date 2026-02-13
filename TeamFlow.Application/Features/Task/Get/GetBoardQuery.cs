using MediatR;

public record GetBoardQuery(Guid ProjectId)
    : IRequest<BoardDto>;
