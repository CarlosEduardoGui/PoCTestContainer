using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;

namespace PoCTestContainer.API.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ISqlInterface _sqlInterface;

    public UsuarioRepository(ISqlInterface sqlInterface)
    {
        _sqlInterface = sqlInterface;
    }

    public async Task<Usuario> Atualizar(Usuario usuario)
    {

        await _sqlInterface.Execute("UPDATE Usuario SET Nome = @Nome, Sobrenome = @Sobrenome, AtualizadoEm = @AtualizadoEm", new { usuario.Nome, usuario.Sobrenome, usuario.AtualizadoEm });

        return usuario;
    }

    public async Task<Usuario?> Buscar(int id)
    {
        return await _sqlInterface.QueryResult<Usuario>("SELECT * FROM Usuario WHERE Id = @id", new { id });
    }

    public async Task Excluir(int id)
    {
        await _sqlInterface.Execute("DELETE FROM Usuario WHERE Id = @id", new { id });
    }

    public async Task<Usuario> Inserir(Usuario usuario)
    {
        var idUsuario = await _sqlInterface.Execute(
            @"INSERT INTO Usuario(Nome, Sobrenome, CriadoEm, AtualizadoEm) VALUES (@Nome, @Sobrenome, @CriadoEm, NULL);
                SELECT IDENT_CURRENT('Usuario');",
            new { usuario.Nome, usuario.Sobrenome, usuario.CriadoEm });

        usuario.Id = idUsuario;

        return usuario;
    }

    public async Task<List<Usuario>> ListarTodos()
    {
        var usuarios = await _sqlInterface.QueryArrayResult<Usuario>("SELECT * FROM Usuario");

        return usuarios.ToList();
    }
}
