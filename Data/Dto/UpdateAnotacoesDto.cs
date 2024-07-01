using System.ComponentModel.DataAnnotations;
using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class UpdateAnotacoesDto
{
    [Required]
    [StringLength(100)]
    public string tituloAnotacao { get; set; }

    [Required]
    public TipoAnotacao TipoAnotacao { get; set; }

    public IFormFile? Arquivo { get; set; } // Para áudio e desenho

    public string? ConteudoTexto { get; set; } // Para texto
}

