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
public class TarefasController : ControllerBase
{
    private readonly EntidadesContext _entidadesContext;
    private IMapper _mapper;
    private readonly TarefasService _tarefasService;

    public TarefasController(EntidadesContext entidadesContext,
        IMapper mapper, TarefasService tarefasService)
    {
        _entidadesContext = entidadesContext;
        _mapper = mapper;
        _tarefasService = tarefasService;
    }


    [HttpPost]
    [Authorize("AuthenticationUser")]
    public async Task<IActionResult> CadastrarTarefa([FromBody]
    CreateTarefaDto createTarefaDto)
    {
        try
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _tarefasService.CriarTarefa(createTarefaDto, userId);
            return Ok("Tarefa criada com sucesso!");

        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadTarefaDto> GetTarefas(
        [FromQuery] string? tituloTarefa = null)
    {
        //// Obtenha o ID do usuário autenticado
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //// Obtenha os dados do usuário do banco de dados usando o ID
        //var usuario = _entidadesContext.Users.FirstOrDefault(u => u.Id == userId);

        //if (usuario == null)
        //{
        //    return NotFound();
        //}


        if (tituloTarefa == null)
        {
            return _mapper.Map<List<ReadTarefaDto>>(
                _entidadesContext.Tarefas.ToList());
        }


        string[] caracteres = tituloTarefa.Split(' ');

        string primeiraPalavra = caracteres[0];

        return _mapper.Map<List<ReadTarefaDto>>(_entidadesContext.Tarefas
            .Where(tarefa => tarefa.tituloTarefa.StartsWith(primeiraPalavra))
            .ToList());

    }

    [HttpGet("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetTarefasId(int id)
    {
        var tarefa = _entidadesContext.Tarefas.FirstOrDefault(
            tarefa => tarefa.Id == id);
        if (tarefa == null) return NotFound();

        var tarefaDto = _mapper.Map<ReadTarefaDto>(tarefa);
        return Ok(tarefaDto);
    }

    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateTarefa(int id,
        [FromBody] UpdateTarefaDto updateTarefaDto)
    {
        var tarefa = _entidadesContext.Tarefas.FirstOrDefault(
            tarefa => tarefa.Id == id);
        if (tarefa == null) return NotFound();

        _mapper.Map(updateTarefaDto, tarefa);
        _entidadesContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteTarefa(int id)
    {
        var tarefa = _entidadesContext.Tarefas.FirstOrDefault(tarefa => tarefa.Id == id);
        if (tarefa == null) return NotFound();

        _entidadesContext.Remove(tarefa);
        _entidadesContext.SaveChanges();
        return NoContent();

    }





}
