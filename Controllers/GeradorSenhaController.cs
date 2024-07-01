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
public class GeradorSenhaController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private readonly IMapper _mapper;
    private GeradorSenhasService _geradorSenhaService;

    public GeradorSenhaController(EntidadesContext entidadesContext,
        IMapper mapper, GeradorSenhasService geradorSenhaService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _geradorSenhaService = geradorSenhaService;
    }

    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> GerarSenha([FromBody] CreateGeradorSenhasDto
        createGeradorSenhasDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _geradorSenhaService.GerarSenhaAsync(createGeradorSenhasDto, userId);
            return Ok("Senha gerada com sucesso!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadGeradorSenhaDto> GetSenhas([FromQuery] string? redeSocial = null)
    {
        var senhas = _entidadesContext.GeradorSenhas.AsEnumerable();

        if (redeSocial != null)
        {
            string[] palavrasRedeSocial = redeSocial.Split(' ');
            string primeiraPalavra = palavrasRedeSocial[0];
            senhas = senhas.Where(gerador => gerador.RedeSocial.StartsWith(primeiraPalavra));
        }

        var senhasDto = _mapper.Map<List<ReadGeradorSenhaDto>>(senhas.ToList());

        // Descriptografa as senhas antes de retornar
        foreach (var senhaDto in senhasDto)
        {
            senhaDto.GerarSenha = _geradorSenhaService.DecryptString(senhaDto.GerarSenha);
        }

        return senhasDto;
    }


    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteSenhaGerada(int id)
    {
        var senhaGerada = _entidadesContext.GeradorSenhas.FirstOrDefault(senha => senha.Id == id);
        if (senhaGerada == null) return NotFound();

        _entidadesContext.Remove(senhaGerada);
        _entidadesContext.SaveChanges();
        return NoContent();
    }

}

