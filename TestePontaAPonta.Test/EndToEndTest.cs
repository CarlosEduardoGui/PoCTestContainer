using Bogus;
using Microsoft.Extensions.DependencyInjection;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;

namespace TestePontaAPonta.Test;
public abstract class EndToEndTest : IClassFixture<CustomApiFactory>, IDisposable
{
    private readonly Faker _faker;
    private readonly IServiceScope _scope;
    protected readonly IUsuarioRepository _usuarioRepository;
    protected readonly HttpClient _httpClient;

    protected EndToEndTest(CustomApiFactory factory)
    {
        _faker = new Faker("pt_BR");

        _scope = factory.Services.CreateScope();
        var scope = factory.Services.CreateScope();
        var bancoDados = scope.ServiceProvider.GetRequiredService<ICriarBancoDados>();
        bancoDados.CriarBanco();

        _httpClient = factory.CreateClient();
        _usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
    }

    public async Task InserirUsuarios()
    {
        var usuarios = CriarUsuarios();
        foreach (var usuario in usuarios)
        {
            await _usuarioRepository.Inserir(usuario);
        }
    }

    public async Task InserirUsuario(Usuario usuario) 
        => await _usuarioRepository.Inserir(usuario);

    public Usuario CriarUsuario()
        => new(_faker.Name.FindName(), _faker.Name.LastName());

    private IEnumerable<Usuario> CriarUsuarios()
        => Enumerable
        .Range(1, 5)
        .Select(_ => CriarUsuario());

    public void Dispose()
    {
        _scope?.Dispose();
        _httpClient?.Dispose();
    }
}
