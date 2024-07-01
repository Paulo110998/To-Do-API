using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public enum RecurrenceType
{
    Hoje, // 0
    Amanha, //1
    ProximaSemana, // 2
    Personalizado // 3
}

public class Tarefas : ICloneable
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? tituloTarefa { get; set; }

    [Required]
    public DateTime dataInicio { get; set; }

    [Required]
    public DateTime dataConclusão { get; set; }

    public RecurrenceType? Repeticao { get; set; }

    public int? ListasId { get; set; }
    public virtual Listas Listas { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } // type -> varchar(127) NOT NULL

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}