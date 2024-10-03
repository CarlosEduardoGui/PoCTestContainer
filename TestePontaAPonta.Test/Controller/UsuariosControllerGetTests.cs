using FluentAssertions;
using Newtonsoft.Json;
using PoCTestContainer.API.Models;

namespace TestePontaAPonta.Test.Controller;

public class UsuariosControllerGetTests : EndToEndTest
{
    public UsuariosControllerGetTests(CustomApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DadoQueBuscarTodosUsuarios_QuandoExistirUsuarios_EntaoRetornarTodos()
    {
        await InserirUsuarios();
        
        var response = await _httpClient.GetAsync("/usuarios/api/v1/todos");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().NotBeNullOrEmpty();
        var usuarios = JsonConvert.DeserializeObject<IEnumerable<Usuario>>(responseString);
        usuarios.Should().HaveCount(5);
        var usuariosBancoDados = await _usuarioRepository.ListarTodos();
        usuariosBancoDados.Should().BeEquivalentTo(usuarios);
    }
}
