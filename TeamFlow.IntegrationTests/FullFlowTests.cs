using FluentAssertions;
using System.Net;
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
    public async Task Should_Return_400_When_Project_Name_Is_Empty()
    {
        var token = await RegisterAuthorizeAndReturnToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync(
            "/api/projects",
            new
            {
                Name = "",
                Description = "Test"
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_Register_And_Authorize_Successfully()
    {
        var registerResponse = await _client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                email = "flow@test.com",
                password = "Password123!"
            });

        registerResponse.IsSuccessStatusCode.Should().BeTrue();

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
    }

    private async Task<string> RegisterAuthorizeAndReturnToken()
    {
        await _client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                email = "flow@test.com",
                password = "Password123!"
            });

        var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new
            {
                email = "flow@test.com",
                password = "Password123!"
            });

        var loginContent = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        return loginContent!.AccessToken;
    }

    [Fact]
    public async Task Full_Project_Task_Board_Flow_Should_Work()
    {
        var token = await RegisterAuthorizeAndReturnToken();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var projectRequest = await _client.PostAsJsonAsync(
            "/api/projects", new CreateProjectRequest("Test Project", "Test desc"));
        projectRequest.IsSuccessStatusCode.Should().BeTrue();

        var createProjectResponse = await projectRequest.Content
            .ReadFromJsonAsync<CreateProjectResponse>();

        var taskRequest = await _client.PostAsJsonAsync(
            "/api/tasks",
            new CreateTaskRequest(
                createProjectResponse!.ProjectId,
                "Test Task",
                "Task desc"));

        taskRequest.IsSuccessStatusCode.Should().BeTrue();

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
