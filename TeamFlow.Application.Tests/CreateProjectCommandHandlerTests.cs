using FluentAssertions;
using Moq;
using TeamFlow.Application.Common.Exceptions;
using TeamFlow.Application.Common.Interfaces;
using TeamFlow.Application.Features.Projects.Create;
using TeamFlow.Domain.Entities;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _repoMock = new();
    private readonly Mock<ICurrentUserService> _userMock = new();

    [Fact]
    public async Task Should_Create_Project_And_Return_Id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userMock.Setup(x => x.UserId).Returns(userId);

        var handler = new CreateProjectCommandHandler(
            _repoMock.Object,
            _userMock.Object);

        var command = new CreateProjectCommand("Test", "Desc");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _repoMock.Verify(x =>
            x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _repoMock.Verify(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Unauthorized_When_User_Not_Owner()
    {
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var taskRepo = new Mock<ITaskRepository>();
        var projectRepo = new Mock<IProjectRepository>();
        var userService = new Mock<ICurrentUserService>();
        var cacheService = new Mock<ICacheService>();

        var task = new TaskItem("Title", null, projectId);

        taskRepo.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        projectRepo.Setup(x => x.GetByIdAsync(projectId))
            .ReturnsAsync(new Project("Test", null, Guid.NewGuid())); 

        userService.Setup(x => x.UserId)
            .Returns(Guid.NewGuid());

        var handler = new UpdateTaskStatusCommandHandler(
            taskRepo.Object,
            projectRepo.Object,
            userService.Object,
            cacheService.Object);

        var command = new UpdateTaskStatusCommand(taskId, TeamFlow.Domain.Enums.TaskStatus.Done);

        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
