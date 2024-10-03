using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace TestePontaAPonta.Test.Controller;
public class UsuariosControllerPutTests : EndToEndTest
{
    public UsuariosControllerPutTests(CustomApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DadoQueAtualizarUsuario_QuandoUsuarioValido_EntaoNaoRetornarNadaMasAdicionarDataAlteracaoNaColunaDoUsuario()
    {
        var usuario = CriarUsuario();
        usuario = await InserirUsuario(usuario);
        var usuarioDadoAtualizado = AtualizarDadoUsuario();

        var response = await _httpClient.PutAsJsonAsync($"usuarios/api/v1/atualizar?id={usuario.Id}", usuarioDadoAtualizado);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().BeNullOrEmpty();
        var usuarioBancoDados = await _usuarioRepository.Buscar(usuario.Id);
        usuarioBancoDados.Should().NotBeNull();
        usuarioBancoDados!.Id.Should().Be(usuario.Id);
        usuarioBancoDados.Nome.Should().Be(usuarioDadoAtualizado.Nome);
        usuarioBancoDados.Sobrenome.Should().Be(usuarioDadoAtualizado.Sobrenome);
        usuarioBancoDados.CriadoEm.Should().NotBeSameDateAs(default);
        usuarioBancoDados.AtualizadoEm.Should().NotBeNull();
    }
}
