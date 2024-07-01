using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;

public class UpdateFamiliaDto
{
    [Required]
    [StringLength(100)]
    public string TituloRedeFamilia { get; set; }
}
