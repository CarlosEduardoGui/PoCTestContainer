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
        var usuarioId = JsonConvert.DeserializeObject<int>(responseString);
        usuarioId.Should().Be(6);
        var usuarioBancoDados = await _usuarioRepository.Buscar(usuarioId);
        usuarioBancoDados.Should().NotBeNull();
        usuarioBancoDados!.Id.Should().Be(usuarioId);
        usuarioBancoDados.Nome.Should().Be(usuario.Nome);
        usuarioBancoDados.Sobrenome.Should().Be(usuario.Sobrenome);
        usuarioBancoDados.CriadoEm.Should().BeOnOrAfter(usuario.CriadoEm);
        usuarioBancoDados.AtualizadoEm.Should().Be(usuario.AtualizadoEm);
    }
}
