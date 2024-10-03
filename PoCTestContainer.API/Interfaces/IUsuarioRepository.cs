using PoCTestContainer.API.Models;

namespace PoCTestContainer.API.Interfaces;

public interface IUsuarioRepository
{
    Task<List<Usuario>> ListarTodos();
    Task<Usuario?> Buscar(int id);
    Task<Usuario> Inserir(Usuario usuario);
    Task<Usuario> Atualizar(Usuario usuario);
    Task Excluir(int id);
}
