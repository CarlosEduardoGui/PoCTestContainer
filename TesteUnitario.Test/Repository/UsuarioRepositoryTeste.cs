using NSubstitute;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;
using PoCTestContainer.API.Repository;

namespace TesteUnitario.Test.Repository;

public class UsuarioRepositoryTeste
{
    private ISqlInterface _sqlInterface;

    [SetUp]
    public void SetUp()
    {
        _sqlInterface = Substitute.For<ISqlInterface>();
    }

    [Test]
    public async Task DadoQueNovoUsuario_QuandoUsuarioValido_EntaoSalvar()
    {
        var usuario = new Usuario("Carlos", "Guimaraes");
        _sqlInterface
            .Execute(Arg.Any<string>(), Arg.Any<object>())
            .Returns(1);
        var repositorio = new UsuarioRepository(_sqlInterface);

        var usuarioInserido = await repositorio.Inserir(usuario);

        Assert.Multiple(() =>
        {
            Assert.That(usuarioInserido.Id, Is.EqualTo(1));
            Assert.That(usuarioInserido.Nome, Is.EqualTo(usuario.Nome));
            Assert.That(usuarioInserido.Sobrenome, Is.EqualTo(usuario.Sobrenome));
            Assert.That(usuarioInserido.CriadoEm, Is.Not.EqualTo(default(DateTime)));
            Assert.That(usuarioInserido.AtualizadoEm, Is.Null);
        });
    }

    [Test]
    public async Task DadoQueBuscarUsuario_QuandoUsuarioExistir_EntaoRetornarUsuario()
    {
        var usuario = new Usuario("Carlos", "Guimaraes")
        {
            Id = 1
        };
        _sqlInterface
            .QueryResult<Usuario>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(usuario);
        var repositorio = new UsuarioRepository(_sqlInterface);

        var usuarioBanco = await repositorio.Buscar(usuario.Id);

        Assert.Multiple(() =>
        {
            Assert.That(usuarioBanco, Is.Not.Null);
            Assert.That(usuarioBanco!.Id, Is.EqualTo(1));
            Assert.That(usuarioBanco.Nome, Is.EqualTo(usuario.Nome));
            Assert.That(usuarioBanco.Sobrenome, Is.EqualTo(usuario.Sobrenome));
            Assert.That(usuarioBanco.CriadoEm, Is.Not.EqualTo(default(DateTime)));
            Assert.That(usuarioBanco.AtualizadoEm, Is.Null);
        });
    }

    [Test]
    public async Task DadoQueAtualizarUsuario_QuandoUsuarioValido_EntaoRetornarUsuario()
    {
        var usuario = new Usuario("Carlos", "Guimaraes");
        usuario.MudarNomeESobrenome("Eduardo", "De Souza");
        _sqlInterface
            .Execute(Arg.Any<string>(), Arg.Any<object>())
            .Returns(1);
        var repositorio = new UsuarioRepository(_sqlInterface);

        var usuarioAtualizado = await repositorio.AtualizarUsuario(usuario);

        Assert.Multiple(() =>
        {
            Assert.That(usuarioAtualizado.Id, Is.EqualTo(0));
            Assert.That(usuarioAtualizado.Nome, Is.EqualTo(usuario.Nome));
            Assert.That(usuarioAtualizado.Sobrenome, Is.EqualTo(usuario.Sobrenome));
            Assert.That(usuarioAtualizado.CriadoEm, Is.Not.EqualTo(default(DateTime)));
            Assert.That(usuarioAtualizado.AtualizadoEm, Is.Not.EqualTo(default(DateTime)));
        });
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
            );
        _sqlInterface
           .QueryArrayResult<Usuario>(Arg.Any<string>())
           .Returns(usuarios);
        var repositorio = new UsuarioRepository(_sqlInterface);

        var usuarioLista = await repositorio.ListarTodos();

        Assert.Multiple(() =>
        {
            Assert.That(usuarios.Count(), Is.EqualTo(5));
        });
    }
}
