namespace TO_DO___API.Data.Dto;

public class ReadFamiliaDto
{
    public int Id { get; set; }

    public string TituloRedeFamilia { get; set; }

    public string CreatedByUserId { get; set; }

    public ICollection<ReadMembroFamiliaDto> ReadMembrosFamilias { get; set; }
}
