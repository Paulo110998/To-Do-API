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
public class FamiliaController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private IMapper _mapper;
    private readonly FamiliaService _familiaService;

    public FamiliaController(EntidadesContext entidadesContext,
        IMapper mapper, FamiliaService familiaService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _familiaService = familiaService;
    }

    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> CadastrarFamilia([FromBody] CreateFamiliaDto
        createFamiliaDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _familiaService.CadastrarFamiliaAsync(createFamiliaDto, userId);
            return Ok("Família cadastrada com sucesso!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadFamiliaDto> GetFamilias(
        [FromQuery] string? tituloRedeFamilia = null)
    {
        if (tituloRedeFamilia == null)
        {
            return _mapper.Map<List<ReadFamiliaDto>>(
                _entidadesContext.Familia.ToList());
        }

        string[] caracteres = tituloRedeFamilia.Split(' ');

        string primeiraPalavra = caracteres[0];

        return _mapper.Map<List<ReadFamiliaDto>>(_entidadesContext.Familia
                    .Where(familia => familia.TituloRedeFamilia.StartsWith(primeiraPalavra))
                    .ToList());
    }


    [HttpGet("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetFamiliaId(int id)
    {
        var familia = _entidadesContext.Familia.FirstOrDefault(
            familia => familia.Id == id);
        if (familia == null) return NotFound();

        var familiaDto = _mapper.Map<ReadFamiliaDto>(familia);
        return Ok(familiaDto);
    }


    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateFamilia(int id,
        [FromBody] UpdateFamiliaDto updateFamiliaDto)
    {
        var familia = _entidadesContext.Familia.FirstOrDefault(
            familia => familia.Id == id);
        if (familia == null) return NotFound();

        _mapper.Map(updateFamiliaDto, familia);
        _entidadesContext.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteFamilia(int id)
    {
        var familia = _entidadesContext.Familia.FirstOrDefault(familia => familia.Id == id);
        if (familia == null) return NotFound();

        _entidadesContext.Remove(familia);
        _entidadesContext.SaveChanges();
        return NoContent();
    }
}
