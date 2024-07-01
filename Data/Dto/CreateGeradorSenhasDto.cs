using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;

public class CreateGeradorSenhasDto
{

    [Required]
    [StringLength(100)]
    public string RedeSocial { get; set; }

    [Required]
    public int QuantidadeCaracteres { get; set; }
}
