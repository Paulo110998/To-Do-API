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
public class ListasController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private IMapper _mapper;
    private readonly ListasService _listasService;

    public ListasController(EntidadesContext entidadesContext,
        IMapper mapper, ListasService listasService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _listasService = listasService;
    }


    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> CadastrarLista([FromBody] CreateListaDto createListaDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _listasService.CriarListaAsync(createListaDto, userId);
            return Ok("Lista cadastrada com sucesso!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadListaDto> GetListas(
        [FromQuery] string? tituloLista = null)
    {
        if (tituloLista == null)
        {
            return _mapper.Map<List<ReadListaDto>>(
                _entidadesContext.Listas.ToList());
        }

        string[] palavrasLista = tituloLista.Split(' ');

        string primeiraPalavra = palavrasLista[0];

        return _mapper.Map<List<ReadListaDto>>(_entidadesContext.Listas
            .Where(lista => lista.tituloLista.StartsWith(primeiraPalavra))
            .ToList());
    }


    [HttpGet("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetListaId(int id)
    {
        var lista = _entidadesContext.Listas.FirstOrDefault(lista => lista.Id == id);
        if (lista == null) return NotFound();
        var listaDto = _mapper.Map<ReadListaDto>(lista);
        return Ok(listaDto);
    }


    //[HttpPut("{id}")]
    //[Authorize("AuthenticationUser")]
    //public IActionResult UpdateLista(int id,
    // [FromBody] UpdateListaDto updateListaDto)
    //{
    //    var lista = _entidadesContext.Listas.FirstOrDefault(
    //        lista => lista.Id == id);
    //    if (lista == null) return NotFound();

    //    _mapper.Map(updateListaDto, lista);
    //    _entidadesContext.SaveChanges();
    //    return NoContent();
    //}

    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> UpdateLista(int id, [FromBody] UpdateListaDto updateListaDto)
    {
        try
        {
            await _listasService.AtualizarListaAsync(id, updateListaDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteLista(int id)
    {
        var lista = _entidadesContext.Listas.FirstOrDefault(lista => lista.Id == id);
        if (lista == null) return NotFound();

        _entidadesContext.Remove(lista);
        _entidadesContext.SaveChanges();
        return NoContent();

    }




}
