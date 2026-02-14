using System.Net.Http.Json;
using FluentAssertions;

public class AuthTests : IClassFixture<PostgresFixture>
{
    private readonly HttpClient _client;

    public AuthTests(PostgresFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(
            fixture.Postgres.GetConnectionString());

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Register_User()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            email = "test@test.com",
            password = "Password123!"
        });

        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
