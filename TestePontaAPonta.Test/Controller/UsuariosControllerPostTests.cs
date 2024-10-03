using FluentAssertions;
using Newtonsoft.Json;
using PoCTestContainer.API.Models;
using System.Net.Http.Json;

namespace TestePontaAPonta.Test.Controller;
public class UsuariosControllerPostTests : EndToEndTest
{
    public UsuariosControllerPostTests(CustomApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DadoQueInserirUsuario_QuandoUsuarioValido_EntaoRetornarUsuarioCriado()
    {
        var usuario = CriarUsuario();
        await InserirUsuarios();

        var response = await _httpClient.PostAsJsonAsync("usuarios/api/v1/inserir", usuario);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().NotBeNullOrEmpty();
        var usuarios = JsonConvert.DeserializeObject<int>(responseString);
    }
}
