using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public class GeradorSenhas
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string RedeSocial { get; set; }

    [Required]
    public string GerarSenha { get; set; }

    [Required]
    public int QuantidadeCaracteres { get; set; }

    [Required]
    public string CreatedByUserId { get; set; }

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }
}
