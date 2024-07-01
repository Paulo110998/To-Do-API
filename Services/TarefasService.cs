using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class TarefasService
{
    private readonly EntidadesContext _entidadesContext;

    public TarefasService(EntidadesContext entidadesContext)
    {
        _entidadesContext = entidadesContext;
    }

    // Método para criar uma nova tarefa
    public async Task CriarTarefa(CreateTarefaDto tarefaDto, string userId)
    {
        // Verifica se o Id do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário.");
        }

        // Cria uma nova tarefa a partir do DTO e do Id do usuário
        Tarefas novaTarefa = new Tarefas()
        {
            tituloTarefa = tarefaDto.tituloTarefa,
            dataInicio = tarefaDto.dataInicio,
            dataConclusão = tarefaDto.dataConclusão,
            Repeticao = tarefaDto.Repeticao,
            ListasId = tarefaDto.ListasId,
            CreatedByUserId = userId
        };

        // Adiciona a nova tarefa ao contexto
        _entidadesContext.Tarefas.Add(novaTarefa);
        await _entidadesContext.SaveChangesAsync();

        // Aplica a lógica de recorrência com base no tipo de repetição
        await AplicarRecorrencia(novaTarefa);
    }

    // Método para aplicar a recorrência conforme o tipo de repetição
    private async Task AplicarRecorrencia(Tarefas tarefa)
    {
        switch (tarefa.Repeticao)
        {
            case RecurrenceType.Hoje:
                // Nada a fazer, a tarefa já está cadastrada na data atual
                break;
            case RecurrenceType.Amanha:
                await CloneAmanha(tarefa);
                break;
            case RecurrenceType.ProximaSemana:
                await CloneProximaSemana(tarefa);
                break;
            case RecurrenceType.Personalizado:
                // Implementar lógica personalizada, se necessário
                break;
        }
    }

    // Método para clonar a tarefa para o próximo dia
    private async Task CloneAmanha(Tarefas tarefa)
    {
        // Calcula a nova data de início e conclusão para a tarefa clonada
        DateTime novaDataInicio = tarefa.dataInicio.AddDays(1);
        DateTime novaDataConclusao = tarefa.dataConclusão.AddDays(1);

        // Cria o clone da tarefa com as novas datas
        Tarefas clone = CriarClone(tarefa, novaDataInicio, novaDataConclusao);
        _entidadesContext.Tarefas.Add(clone);

        // Salva as alterações no banco
        await _entidadesContext.SaveChangesAsync();
    }

    // Método para clonar a tarefa para a próxima semana
    private async Task CloneProximaSemana(Tarefas tarefa)
    {
        // Calcula a nova data de início e conclusão para a tarefa clonada
        DateTime novaDataInicio = tarefa.dataInicio.AddDays(7);
        DateTime novaDataConclusao = tarefa.dataConclusão.AddDays(7);

        // Cria o clone da tarefa com as novas datas
        Tarefas clone = CriarClone(tarefa, novaDataInicio, novaDataConclusao);
        _entidadesContext.Tarefas.Add(clone);

        // Salva as alterações no banco
        await _entidadesContext.SaveChangesAsync();
    }

    // Método auxiliar para criar um clone da tarefa com novas datas
    private Tarefas CriarClone(Tarefas tarefa, DateTime novaDataInicio, DateTime novaDataConclusao)
    {
        // Clona a tarefa original
        var clone = (Tarefas)tarefa.Clone();
        // Reseta o Id para que o EF crie um novo registro
        clone.Id = 0;
        // Define as novas datas de início e conclusão
        clone.dataInicio = novaDataInicio;
        clone.dataConclusão = novaDataConclusao;
        return clone;
    }
}