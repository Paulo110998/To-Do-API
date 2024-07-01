namespace TO_DO___API.Data.Dto;

public class ReadGeradorSenhaDto
{
    public int Id { get; set; }

    public string RedeSocial { get; set; }

    public string GerarSenha { get; set; }
    public int QuantidadeCaracteres { get; set; }

    public string CreatedByUserId { get; set; }
}
