﻿using Microsoft.AspNetCore.Mvc;
using PoCTestContainer.API.Interfaces;
using PoCTestContainer.API.Models;

namespace PoCTestContainer.API.Controllers;

[ApiController]
[Route("[controller]/api/v1")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepositorio;

    public UsuariosController(IUsuarioRepository sqlInterface)
    {
        _usuarioRepositorio = sqlInterface;
    }

    [HttpGet("todos")]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioRepositorio.ListarTodos();

        return Ok(usuarios);
    }

    [HttpPut("atualizar")]
    public async Task<IActionResult> AlterarUsuario([FromQuery] int id, [FromBody] CriarEEditarUsuario novoUsuario)
    {
        var usuario = await _usuarioRepositorio.Buscar(id);
        if (usuario == null)
            return BadRequest($"Usuario {id} não existe.");

        usuario.MudarNomeESobrenome(novoUsuario.Nome, novoUsuario.Sobrenome);

        await _usuarioRepositorio.Atualizar(usuario);

        return NoContent();
    }

    [HttpPost("inserir")]
    public async Task<IActionResult> InserirUsuario([FromBody] CriarEEditarUsuario editarUsuario)
    {
        var usuario = new Usuario(editarUsuario.Nome, editarUsuario.Sobrenome);

        var usuarioInserido = await _usuarioRepositorio.Inserir(usuario);

        return Created("/id", usuarioInserido.Id);
    }

    [HttpDelete("excluir")]
    public async Task<IActionResult> ExcluirUsuario([FromQuery] int id)
    {
        var usuario = await _usuarioRepositorio.Buscar(id);
        if (usuario == null)
            return BadRequest($"Usuario {id} não existe.");

        await _usuarioRepositorio.Excluir(usuario.Id);

        return NoContent();
    }
}
