using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;


public class CreateFamiliaDto
{
    [Required]
    [StringLength(100)]
    public string TituloRedeFamilia { get; set; }
}