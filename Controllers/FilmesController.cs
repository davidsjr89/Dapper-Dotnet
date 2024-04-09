using Dapper_Dotnet.Models;
using Dapper_Dotnet.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Formats.Asn1;

namespace Dapper_Dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmesController : ControllerBase
{
    private readonly IFilmeRepository _filmeRepository;

    public FilmesController(IFilmeRepository filmeRepository)
    {
        _filmeRepository = filmeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var filmes = await _filmeRepository.BuscaFilmesAsync();

        return filmes.Any() 
            ? Ok(filmes) 
            : NoContent();
    }

    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var filme = await _filmeRepository.BuscaFilmeAsync(id);

        return filme != null 
            ? Ok(filme) 
            : NotFound("Filme não encontrado");
    }

    [HttpPost]
    public async Task<IActionResult> Post(FilmeRequest filmeRequest)
    {
        if(string.IsNullOrEmpty(filmeRequest.Nome.Trim()) || filmeRequest.Ano <= 0 || filmeRequest.ProdutoraId <= 0)
        {
            return BadRequest("Informações inválidas");
        }

        var adicionado = await _filmeRepository.AdicionarAsync(filmeRequest);

        return adicionado
                 ? Ok("Filme adicionado com sucesso")
                 : BadRequest("Erro ao adicionar filme");
    }

    [HttpPut]
    public async Task<IActionResult> Put(FilmeRequest filmeRequest, int id)
    {
        if (id <= 0) return BadRequest("Filme inválido");

        var filme = await _filmeRepository.BuscaFilmeAsync(id);

        if (filme == null) return NotFound("Filme não existe");

        if (string.IsNullOrEmpty(filmeRequest.Nome)) filmeRequest.Nome = filme.Nome;
        if (filmeRequest.Ano <= 0 ) filmeRequest.Ano = filme.Ano;

        var atualizado = await _filmeRepository.AtualizarAsync(filmeRequest, id);

        return atualizado
                 ? Ok("Filme atualizado com sucesso")
                 : BadRequest("Erro ao atualizado filme");
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0) return BadRequest("Filme inválido");

        var filme = await _filmeRepository.BuscaFilmeAsync(id);

        if (filme == null) return NotFound("Filme não existe");

        var deletado = await _filmeRepository.DeletarAsync(id);

        return deletado
                 ? Ok("Filme deletado com sucesso")
                 : BadRequest("Erro ao deletado filme");
    }
}
