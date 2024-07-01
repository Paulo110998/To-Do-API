using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class ReadListaDto
{
    public int Id { get; set; }

    public string tituloLista { get; set; }

    public string CorDeFundo { get; set; }

    public string ImagemFundo { get; set; }

    public string ImagemFundoPersonalizar { get; set; }

    public Duplicacao? Duplicacao { get; set; }

    public DateTime? DataCriacao { get; set; }

    public string CreatedByUserId { get; set; }

    public ICollection<ReadTarefaDto> ReadTarefasNaLista { get; set; }
}
