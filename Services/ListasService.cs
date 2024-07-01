using Microsoft.Extensions.Caching.Memory;
using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class ListasService
{
    private readonly EntidadesContext _entidadesContext;
    private readonly IMemoryCache _memoryCache;

    // Construtor que recebe o contexto do banco de dados e o cache de memória
    public ListasService(EntidadesContext entidadesContext, IMemoryCache memoryCache)
    {
        _entidadesContext = entidadesContext;
        _memoryCache = memoryCache;
    }

    // Método para criar uma nova lista
    public async Task CriarListaAsync(CreateListaDto createListaDto, string userId)
    {
        // Verifica se o ID do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário autenticado");
        }

        // Cria uma nova lista a partir dos dados do DTO
        Listas novaLista = new Listas
        {
            tituloLista = createListaDto.tituloLista,
            CorDeFundo = createListaDto.CorDeFundo,
            ImagemFundo = createListaDto.ImagemFundo,
            ImagemFundoPersonalizar = createListaDto.ImagemFundoPersonalizar,
            Duplicacao = createListaDto.Duplicacao,
            DataCriacao = createListaDto.DataCriacao,
            CreatedByUserId = userId,
            TarefasNaLista = new List<Tarefas>() // Inicializa a lista de tarefas
        };

        // Adiciona a nova lista ao contexto
        _entidadesContext.Listas.Add(novaLista);
        // Salva as mudanças no banco de dados
        await _entidadesContext.SaveChangesAsync();

        // Adiciona a lista ao cache
        _memoryCache.Set(novaLista.Id, novaLista);

        // Verifica se a lista deve ser duplicada
        if (novaLista.Duplicacao == Duplicacao.Duplicar)
        {
            // Chama o método para duplicar a lista
            await DuplicarListaAsync(novaLista);
        }
    }

    // Método para atualizar uma lista existente
    public async Task AtualizarListaAsync(int id, UpdateListaDto updateListaDto)
    {
        // Encontra a lista no banco de dados, incluindo suas tarefas associadas
        var lista = _entidadesContext.Listas.Include(l => l.TarefasNaLista)
                                            .FirstOrDefault(l => l.Id == id);

        // Se a lista não for encontrada, lança uma exceção
        if (lista == null)
        {
            throw new ArgumentNullException("Lista não encontrada");
        }

        // Atualiza os campos da lista com os dados do DTO
        lista.tituloLista = updateListaDto.tituloLista;
        lista.CorDeFundo = updateListaDto.CorDeFundo;
        lista.ImagemFundo = updateListaDto.ImagemFundo;
        lista.ImagemFundoPersonalizar = updateListaDto.ImagemFundoPersonalizar;
        lista.Duplicacao = updateListaDto.Duplicacao;

        // Salva as mudanças no banco de dados
        await _entidadesContext.SaveChangesAsync();

        // Verifica se a lista deve ser duplicada
        if (lista.Duplicacao == Duplicacao.Duplicar)
        {
            // Chama o método para duplicar a lista
            await DuplicarListaAsync(lista);
        }
    }

    // Método para duplicar uma lista
    private async Task DuplicarListaAsync(Listas lista)
    {
        // Clona a lista mantendo a data de criação
        var clone = (Listas)lista.Clone();
        // Define o ID do clone como 0 para que o banco de dados gere um novo ID
        clone.Id = 0;

        // Clona as tarefas associadas à lista
        clone.TarefasNaLista = lista.TarefasNaLista.Select(t =>
        {
            // Clona cada tarefa
            var clonedTask = (Tarefas)t.Clone();
            // Define o ID da tarefa clonada como 0 para que o banco de dados gere um novo ID
            clonedTask.Id = 0;
            return clonedTask;
        }).ToList();

        // Adiciona a lista clonada ao contexto
        _entidadesContext.Listas.Add(clone);
        // Salva as mudanças no banco de dados
        await _entidadesContext.SaveChangesAsync();
    }

    // Método para obter uma lista do cache
    public Listas GetListaFromCache(int id)
    {
        _memoryCache.TryGetValue(id, out Listas lista);
        return lista;
    }
}