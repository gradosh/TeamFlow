using FluentAssertions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TeamFlow.API.Contracts.Projects;
using TeamFlow.Application.Features.Projects.Create;
using TeamFlow.Contracts.Tasks;

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}

public class FullFlowTests : IClassFixture<PostgresFixture>
{
    private readonly HttpClient _client;

    public FullFlowTests(PostgresFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Full_Project_Task_Board_Flow_Should_Work()
    {
        // 1️⃣ Register
        var registerResponse = await _client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                email = "flow@test.com",
                password = "Password123!"
            });

        registerResponse.IsSuccessStatusCode.Should().BeTrue();

        // 2️⃣ Login
        var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new
            {
                email = "flow@test.com",
                password = "Password123!"
            });

        loginResponse.IsSuccessStatusCode.Should().BeTrue();

        var loginContent = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        loginContent.Should().NotBeNull();
        loginContent!.AccessToken.Should().NotBeNullOrEmpty();
        loginContent!.RefreshToken.Should().NotBeNullOrEmpty();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginContent.AccessToken);

        // 3️⃣ Create Project
        var projectRequest = await _client.PostAsJsonAsync(
            "/api/projects",new CreateProjectRequest("Test Project", "Test desc"));
        projectRequest.IsSuccessStatusCode.Should().BeTrue();

        var createProjectResponse = await projectRequest.Content
            .ReadFromJsonAsync<CreateProjectResponse>();

        // 4️⃣ Create Task
        var taskRequest = await _client.PostAsJsonAsync(
            "/api/tasks", 
            new CreateTaskRequest(
                createProjectResponse!.ProjectId, 
                "Test Task", 
                "Task desc"));

        taskRequest.IsSuccessStatusCode.Should().BeTrue();

        // 5️⃣ Get Board
        var boardRequest = await _client.GetAsync(
            $"/api/tasks/board/{createProjectResponse?.ProjectId}");

        boardRequest.IsSuccessStatusCode.Should().BeTrue();

        var board = await boardRequest.Content
            .ReadFromJsonAsync<BoardDto>();

        board.Should().NotBeNull();
        board!.Todo.Should().HaveCount(1);
        board.InProgress.Should().BeEmpty();
        board.Done.Should().BeEmpty();
    }
}
