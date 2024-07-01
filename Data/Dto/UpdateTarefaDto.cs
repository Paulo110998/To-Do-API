using System.ComponentModel.DataAnnotations;
using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class UpdateTarefaDto
{

    [Required]
    [StringLength(100)]
    public string tituloTarefa { get; set; }

    [Required]
    public DateTime dataInicio { get; set; }

    [Required]
    public DateTime dataConclusão { get; set; }

    public RecurrenceType? Repeticao { get; set; }

    public int? ListasId { get; set; }
}
