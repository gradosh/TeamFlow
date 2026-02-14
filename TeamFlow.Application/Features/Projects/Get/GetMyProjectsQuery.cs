using MediatR;
using TeamFlow.Domain.Entities;

public record GetMyProjectsQuery() : IRequest<List<Project>>;
