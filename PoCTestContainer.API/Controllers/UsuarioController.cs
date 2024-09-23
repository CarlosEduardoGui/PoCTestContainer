using Microsoft.AspNetCore.Mvc;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;

namespace PoCTestContainer.API.Controllers;

[ApiController]
[Route("[controller]/v1/api")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepositorio;

    public UsuarioController(IUsuarioRepository sqlInterface)
    {
        _usuarioRepositorio = sqlInterface;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioRepositorio.ListarTodos();

        return Ok(usuarios);
    }

    [HttpPost("atualizar")]
    public async Task<IActionResult> AlterarUsuario([FromQuery] int id, [FromBody] CriarEEditarUsuario novoUsuario)
    {
        var usuario = await _usuarioRepositorio.Buscar(id);
        if (usuario == null)
            return BadRequest($"Usuario {id} não existe.");

        usuario.MudarNomeESobrenome(novoUsuario.Nome, novoUsuario.Sobrenome);

        await _usuarioRepositorio.AtualizarUsuario(usuario);

        return Ok(usuario);
    }

    [HttpPost("inserir")]
    public async Task<IActionResult> InserirUsuario([FromBody] CriarEEditarUsuario editarUsuario)
    {
        var usuario = new Usuario(editarUsuario.Nome, editarUsuario.Sobrenome);

        var usuarioInserido = await _usuarioRepositorio.Inserir(usuario);

        return Created("/id", usuarioInserido.Id);
    }
}
