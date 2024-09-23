using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;
using System.Data;
using Testcontainers.MsSql;

namespace TestePontaAPonta.Test.Controller;

public class UsuarioControllerTeste
{
    private const string Database = "master";
    private const string Username = "sa";
    private const string Password = "$trongPassword";
    private const ushort MsSqlPort = 1433;

    private WebApplicationFactory<Program> _factory;
    private IContainer _container;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Environment.SetEnvironmentVariable("DOCKER_HOST", "unix:///var/run/docker.sock");
        _container = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        //Start Container
        await _container.StartAsync();

        var host = _container.Hostname;
        var port = _container.GetMappedPublicPort(MsSqlPort);

        // Replace connection string in DbContext
        var connectionString = $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDbConnection>(imp => new SqlConnection(connectionString));
                });
            });

        // Initialize database
        using var scope = _factory.Services.CreateScope();
        var createDatabaseService = scope.ServiceProvider.GetRequiredService<ICriarBancoDados>();
        createDatabaseService.CriarBanco();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
        _factory.Dispose();
    }

    [Test]
    public async Task GetAllResidents_ReturnsEmptyList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("usuario/api/v1");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var allresidentsResponse = JsonConvert.DeserializeObject<List<Usuario>>(content);
    }
}
