using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestePontaAPonta.Test.Controller;
public class UsuariosControllerDeleteTests : EndToEndTest
{
    public UsuariosControllerDeleteTests(CustomApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DadoQueExcluirUsuario_QuandoUsuarioExistir_EntaoNaoRetornarNadaEUsuarioNaoExistirNaBaseDados()
    {
        var usuario = CriarUsuario();
        usuario = await InserirUsuario(usuario);

        var response = await _httpClient.DeleteAsync($"usuarios/api/v1/excluir?id={usuario.Id}");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().BeNullOrEmpty();
        var usuarioBancoDados = await _usuarioRepository.Buscar(usuario.Id);
        usuarioBancoDados.Should().BeNull();
    }
}
