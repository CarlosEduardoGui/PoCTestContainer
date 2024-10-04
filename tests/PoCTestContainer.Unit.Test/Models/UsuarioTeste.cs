using PoCTestContainer.API.Models;

namespace TesteUnitario.Test.Models;

public class UsuarioTeste
{
    [Test]
    public void DadoQueNovaInstanciaUsuario_QuandoDadosValidos_RetornarInstancia()
    {
        Assert.DoesNotThrow(() => new Usuario("Carlos", "Guimaraes"));
    }

    [Test]
    public void DadoQueInstanciaUsuario_QuandoMudarNomeESobrenome_RetornarDadosModificados()
    {
        var usuario = new Usuario("Carlos", "Guimaraes");

        usuario.MudarNomeESobrenome("Eduardo", "De Souza");

        Assert.Multiple(() =>
        {
            Assert.That(usuario.Nome, Is.EqualTo("Eduardo"));
            Assert.That(usuario.Sobrenome, Is.EqualTo("De Souza"));
            Assert.That(usuario.AtualizadoEm, Is.Not.Null);
        });
    }
}
