using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public enum Duplicacao
{
    Não, // 0
    Duplicar // 1

}

public class Listas : ICloneable
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string tituloLista { get; set; }

    [Required]
    public string CorDeFundo { get; set; }

    [Required]
    public string ImagemFundo { get; set; }

    [Required]
    public string ImagemFundoPersonalizar { get; set; }

    public Duplicacao? Duplicacao { get; set; }

    public DateTime? DataCriacao { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } // type -> varchar(127) NOT NULL

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }

    public virtual ICollection<Tarefas> TarefasNaLista { get; set; }

    public object Clone()
    {
        var clone = (Listas)this.MemberwiseClone();
        clone.TarefasNaLista = new List<Tarefas>(this.TarefasNaLista.Select(t => (Tarefas)t.Clone()));
        return clone;
    }

}