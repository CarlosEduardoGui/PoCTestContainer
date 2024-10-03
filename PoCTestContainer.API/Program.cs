using Microsoft.Data.SqlClient;
using PoCTestContainer.API.BancoDados;
using PoCTestContainer.API.InicializadorBancoDados;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Repository;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<IDbConnection>(imp => new SqlConnection(builder.Configuration.GetConnectionString("PoCTestContainer")));
builder.Services.AddSingleton<ISqlInterface, DbConnection>();

if (builder.Environment.Equals("PontaAPonta") is false)
{
    builder.Services.AddScoped<ICriarBancoDados, CriarBancoDados>();

    var serviceProvider = builder.Services.BuildServiceProvider();
    var createDatabaseService = serviceProvider.GetService<ICriarBancoDados>();
    createDatabaseService!.CriarBanco();
}

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }