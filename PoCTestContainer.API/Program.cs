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
builder.Services.AddScoped<ISqlInterface, DbConnection>();
builder.Services.AddScoped<ICriarBancoDados, CriarBancoDados>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var serviceProvider = builder.Services.BuildServiceProvider();
var createDatabaseService = serviceProvider.GetService<ICriarBancoDados>();

if (createDatabaseService is null)
    throw new NullReferenceException("Não foi possível recuperar um objeto de serviço para criação da estrutura relacional");

createDatabaseService.CriarBanco();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();