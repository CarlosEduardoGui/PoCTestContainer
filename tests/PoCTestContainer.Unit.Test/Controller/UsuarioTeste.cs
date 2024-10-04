using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PoCTestContainer.API.Controllers;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;
using PoCTestContainer.API.Repository;

namespace TesteUnitario.Test.Controller;

public class UsuarioTeste
{
    private IUsuarioRepository _usuarioRepostiroy;

    [SetUp]
    public void SetUp()
    {
        _usuarioRepostiroy = Substitute.For<IUsuarioRepository>();
    }

    [Test]
    public async Task DadoQueNovoUsuario_QuandoUsuarioValido_EntaoSalvar()
    {
        var usuarioRequest = new CriarEEditarUsuario()
        {
            Nome = "Carlos",
            Sobrenome = "Eduardo"
        };
        var usuario = new Usuario(usuarioRequest.Nome, usuarioRequest.Sobrenome)
        {
            Id = 1,
        };
        _usuarioRepostiroy
            .Inserir(Arg.Any<Usuario>())
            .Returns(usuario);
        var controller = new UsuariosController(_usuarioRepostiroy);

        var objectResult = (CreatedResult)await controller.InserirUsuario(usuarioRequest);
        var usuarioCriadoId = (int)objectResult.Value!;

        Assert.Multiple(async () =>
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.That(objectResult.Value, Is.EqualTo(1));
            Assert.That(usuarioCriadoId, Is.EqualTo(usuario.Id));
            await _usuarioRepostiroy
                .Received(1)
                .Inserir(Arg.Is<Usuario>(x =>
                    x.Id == 0
                    && x.Nome == usuario.Nome
                    && x.Sobrenome == usuario.Sobrenome
                    && x.CriadoEm != usuario.CriadoEm
                    && x.AtualizadoEm == default
                ));
        });
    }

    [Test]
    public async Task DadoQueAtualizarUsuario_QuandoUsuarioExistir_EntaoRetornarUsuarioAtualizado()
    {
        var usuarioRequest = new CriarEEditarUsuario()
        {
            Nome = "Carlos",
            Sobrenome = "Guimaraes"
        };
        var usuarioBuscado = new Usuario(usuarioRequest.Nome, usuarioRequest.Sobrenome)
        {
            Id = 1,
        };
        _usuarioRepostiroy
            .Buscar(Arg.Any<int>())
            .Returns(usuarioBuscado);
        var controller = new UsuariosController(_usuarioRepostiroy);

        await controller.AlterarUsuario(usuarioBuscado.Id, usuarioRequest);
    }

    [Test]
    public async Task DadoQueExistirVariosUsuarios_QuandoBuscarTodos_RetornarTodos()
    {
        var usuarios = Enumerable
            .Range(1, 5)
            .Select(_ =>
                new Usuario("Carlos", "Eduardo")
                {
                    Id = new Random().Next()
                }
            ).ToList();
        _usuarioRepostiroy
           .ListarTodos()
           .Returns(usuarios);
        var repositorio = new UsuariosController(_usuarioRepostiroy);

        var usuarioLista = await repositorio.Index();

        Assert.Multiple(() =>
        {
            Assert.That(usuarios.Count(), Is.EqualTo(5));
        });
    }
}
