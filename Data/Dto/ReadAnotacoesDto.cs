using TO_DO___API.Models;

namespace TO_DO___API.Data.Dto;

public class ReadAnotacoesDto
{
    public int Id { get; set; }

    public string tituloAnotacao { get; set; }

    public TipoAnotacao Anotacao { get; set; }

    public string? CaminhoArquivo { get; set; }

    public string? ConteudoTexto { get; set; }

    public string CreatedByUserId { get; set; }
}
