using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class ReadTarefaDto
{
    public int Id { get; set; }

    public string tituloTarefa { get; set; }

    public DateTime dataInicio { get; set; }

    public DateTime dataConclusão { get; set; }

    public RecurrenceType? Repeticao { get; set; }

    public int? ListasId { get; set; }

    public string CreatedByUserId { get; set; }
}
