using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class ReadMembroFamiliaDto
{
    public int Id { get; set; }

    public string NomeMembro { get; set; }

    public VinculoFamilia VinculoFamilia { get; set; }

    public int? FamiliaId { get; set; }

    public string CreatedByUserId { get; set; }

}
