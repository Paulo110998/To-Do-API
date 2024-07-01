using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class AnotacoesService
{
    private readonly EntidadesContext _entidadesContext;

    // Construtor do serviço, que recebe o contexto da entidade via injeção de dependência
    public AnotacoesService(EntidadesContext entidadesContext)
    {
        _entidadesContext = entidadesContext;
    }

    // Método para criar uma nova anotação
    public async Task CriarAnotacao(CreateAnotacoesDto anotacoesDto, string userId)
    {
        // Verifica se o Id do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário.");
        }

        // Cria uma nova instância de Anotacoes com os dados recebidos
        Anotacoes novaAnotacao = new Anotacoes()
        {
            tituloAnotacao = anotacoesDto.tituloAnotacao,
            TipoAnotacao = anotacoesDto.TipoAnotacao,
            CreatedByUserId = userId
        };

        // Verifica o tipo de anotação e executa a lógica correspondente
        switch (anotacoesDto.TipoAnotacao)
        {
            // Se o tipo de anotação for áudio ou desenho, verifica se o arquivo foi enviado e salva o arquivo
            case TipoAnotacao.Audio:
            case TipoAnotacao.Desenho:
                if (anotacoesDto.Arquivo == null)
                {
                    throw new ArgumentNullException("Arquivo não fornecido para o tipo selecionado.");
                }
                // Salva o arquivo e obtém o caminho onde foi salvo
                novaAnotacao.CaminhoArquivo = await SalvarArquivo(anotacoesDto.Arquivo);
                break;

            // Se o tipo de anotação for texto, verifica se o conteúdo do texto foi enviado
            case TipoAnotacao.Texto:
                if (string.IsNullOrEmpty(anotacoesDto.ConteudoTexto))
                {
                    throw new ArgumentNullException("Conteúdo de texto não fornecido.");
                }
                // Atribui o conteúdo do texto à anotação
                novaAnotacao.ConteudoTexto = anotacoesDto.ConteudoTexto;
                break;

            // Lança uma exceção se o tipo de anotação for inválido
            default:
                throw new ArgumentException("Tipo de anotação inválido.");
        }

        // Adiciona a nova anotação ao contexto/banco de dados
        _entidadesContext.Anotacoes.Add(novaAnotacao);
        // Salva as mudanças no banco de dados de forma assíncrona
        await _entidadesContext.SaveChangesAsync();
    }

    // Método privado para salvar o arquivo enviado e retornar o caminho onde foi salvo
    private async Task<string> SalvarArquivo(IFormFile arquivo)
    {
        // Define o caminho onde o arquivo será salvo
        var caminhoPasta = Path.Combine("Uploads", arquivo.FileName);

        // Garante que o diretório onde o arquivo será salvo exista
        Directory.CreateDirectory(Path.GetDirectoryName(caminhoPasta)!);

        // Abre um FileStream para escrever o arquivo no disco
        using (var stream = new FileStream(caminhoPasta, FileMode.Create))
        {
            // Copia o conteúdo do arquivo para o FileStream de forma assíncrona
            await arquivo.CopyToAsync(stream);
        }

        // Retorna o caminho onde o arquivo foi salvo
        return caminhoPasta;
    }
}
