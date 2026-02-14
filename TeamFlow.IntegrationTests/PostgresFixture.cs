using Testcontainers.PostgreSql;

public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Postgres { get; }
    public HttpClient Client { get; private set; } = null!;

    private CustomWebApplicationFactory _factory = null!;

    public PostgresFixture()
    {
        this.Postgres = new PostgreSqlBuilder()
            .WithDatabase("teamflow_test")
            .WithUsername("teamflow")
            .WithPassword("teamflow")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Postgres.StartAsync();

        _factory = new CustomWebApplicationFactory(
            Postgres.GetConnectionString());

        Client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await Postgres.DisposeAsync();
    }
}