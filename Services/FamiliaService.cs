using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class FamiliaService
{
    private readonly EntidadesContext _entidadesContext;

    public FamiliaService(EntidadesContext entidadesContext)
    {
        _entidadesContext = entidadesContext;
    }

    // Método para criar uma nova família
    public async Task CadastrarFamiliaAsync(CreateFamiliaDto familiaDto, string userId)
    {
        // Verifica se o Id do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário.");
        }

        // Cria uma nova família a partir do DTO e do Id do usuário
        Familia novaFamilia = new Familia()
        {
            TituloRedeFamilia = familiaDto.TituloRedeFamilia,
            CreatedByUserId = userId
        };

        // Adiciona a nova familia ao banco de dados
        _entidadesContext.Familia.Add(novaFamilia);
        await _entidadesContext.SaveChangesAsync();
    }
}
