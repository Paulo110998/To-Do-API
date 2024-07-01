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
public class MembrosFamiliaController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private IMapper _mapper;
    private readonly MembroFamiliaService _membroFamiliaService;

    public MembrosFamiliaController(EntidadesContext entidadesContext,
        IMapper mapper, MembroFamiliaService membroFamiliaService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _membroFamiliaService = membroFamiliaService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="createMembroFamiliaDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> CadastrarMembro([FromBody] CreateMembroFamiliaDto
        createMembroFamiliaDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _membroFamiliaService.CadastraMembroAsync(createMembroFamiliaDto, userId);
            return Ok("Membro cadastrado com sucesso!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadMembroFamiliaDto> GetMembros([FromQuery] string?
        nomeMembro = null)
    {
        if (nomeMembro == null)
        {
            return _mapper.Map<List<ReadMembroFamiliaDto>>(
                _entidadesContext.MembrosFamilia.ToList());
        }

        string[] caracteres = nomeMembro.Split(' ');

        string primeiraPalavra = caracteres[0];

        return _mapper.Map<List<ReadMembroFamiliaDto>>(_entidadesContext.MembrosFamilia
            .Where(membro => membro.NomeMembro.StartsWith(primeiraPalavra))
            .ToList());
    }


    [HttpGet("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetMembrosId(int id)
    {
        var membro = _entidadesContext.MembrosFamilia.FirstOrDefault(
            membro => membro.Id == id);
        if (membro == null) return NotFound();

        var membroDto = _mapper.Map<ReadMembroFamiliaDto>(membro);
        return Ok(membroDto);
    }

    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateMembro(int id,
        [FromBody] UpdateMembroFamiliaDto updateMembroFamiliaDto)
    {
        var membro = _entidadesContext.MembrosFamilia.FirstOrDefault(
            membro => membro.Id == id);
        if (membro == null) return NotFound();

        _mapper.Map(updateMembroFamiliaDto, membro);
        _entidadesContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteMembro(int id)
    {
        var membro = _entidadesContext.MembrosFamilia.FirstOrDefault(membro => membro.Id == id);
        if (membro == null) return NotFound();

        _entidadesContext.Remove(membro);
        _entidadesContext.SaveChanges();
        return NoContent();

    }



}