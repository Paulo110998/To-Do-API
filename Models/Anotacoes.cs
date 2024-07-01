using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public enum TipoAnotacao
{
    Audio, // 0
    Desenho, // 1
    Texto // 2
}

public class Anotacoes
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string tituloAnotacao { get; set; }

    [Required]
    public TipoAnotacao TipoAnotacao { get; set; } 

    public string? CaminhoArquivo { get; set; } // Caminho para áudio ou desenho

    public string? ConteudoTexto { get; set; } 

    [Required]
    public string CreatedByUserId { get; set; } 

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }