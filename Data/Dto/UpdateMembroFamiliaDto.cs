using System.ComponentModel.DataAnnotations;
using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class UpdateMembroFamiliaDto
{
    [StringLength(130)]
    public string NomeMembro { get; set; }

    [Required]
    public VinculoFamilia VinculoFamilia { get; set; }

    [Required]
    public int? FamiliaId { get; set; }

    public virtual Familia Familia { get; set; }
}
