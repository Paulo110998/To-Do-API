using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Services;

namespace TO_DO___API.Controllers;

[ApiController]
[Route("[controller]")]
public class AnotacoesController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private readonly IMapper _mapper;
    private readonly AnotacoesService _anotacoesService;

    public AnotacoesController(EntidadesContext entidadesContext,
        IMapper mapper, AnotacoesService anotacoesService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _anotacoesService = anotacoesService;
    }

    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> CadastrarAnotacao([FromForm] CreateAnotacoesDto createAnotacoesDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _anotacoesService.CriarAnotacao(createAnotacoesDto, userId);
            return Ok("Anotação cadastrada com sucesso!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadAnotacoesDto> GetAnotacoes([FromQuery] string? anotacao = null)
    {
        if (anotacao == null)
        {
            return _mapper.Map<List<ReadAnotacoesDto>>(_entidadesContext.Anotacoes.ToList());
        }

        string[] palavrasAnotacao = anotacao.Split(' ');

        string primeiraPalavra = palavrasAnotacao[0];

        return _mapper.Map<List<ReadAnotacoesDto>>(_entidadesContext.Anotacoes
            .Where(anotacoes => anotacoes.tituloAnotacao.StartsWith(primeiraPalavra))
            .ToList());
    }

    [HttpGet("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetAnotacaoId(int id)
    {
        var anotacao = _entidadesContext.Anotacoes.FirstOrDefault(anotacao => anotacao.Id == id);
        if (anotacao == null) return NotFound();
        var anotacaoDto = _mapper.Map<ReadAnotacoesDto>(anotacao);
        return Ok(anotacaoDto);
    }

    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateAnotacao(int id, [FromBody] UpdateAnotacoesDto updateAnotacoesDto)
    {
        var anotacao = _entidadesContext.Anotacoes.FirstOrDefault(anotacao => anotacao.Id == id);
        if (anotacao == null) return NotFound();

        _mapper.Map(updateAnotacoesDto, anotacao);
        _entidadesContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteAnotacao(int id)
    {
        var anotacao = _entidadesContext.Anotacoes.FirstOrDefault(anotacao => anotacao.Id == id);
        if (anotacao == null) return NotFound();

        _entidadesContext.Remove(anotacao);
        _entidadesContext.SaveChanges();
        return NoContent();
    }
}
