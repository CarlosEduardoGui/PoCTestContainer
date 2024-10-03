using System.Data;
using Testcontainers.MsSql;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PoCTestContainer.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PoCTestContainer.API.InicializadorBancoDados;

namespace TestePontaAPonta.Test;

public class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("MSSQL_PID", "Express")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            builder.UseEnvironment("PontaAPonta");

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(IDbConnection));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IDbConnection>(imp => new SqlConnection($"{_dbContainer.GetConnectionString()};MultipleActiveResultSets=true"));
            services.AddScoped<ICriarBancoDados, CriarBancoDados>();
        });
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
