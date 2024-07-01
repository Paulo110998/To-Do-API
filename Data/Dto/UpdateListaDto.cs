using System.ComponentModel.DataAnnotations;
using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class UpdateListaDto
{

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
}