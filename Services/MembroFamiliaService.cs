using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class MembroFamiliaService
{
    private readonly EntidadesContext _entidadesContext;

    public MembroFamiliaService(EntidadesContext entidadesContext)
    {
        _entidadesContext = entidadesContext;
    }

    // Método para criar um novo membro
    public async Task CadastraMembroAsync(CreateMembroFamiliaDto membroFamiliaDto,
        string userId)
    {
        // Verifica se o Id do usuário é válido
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException("Não foi possível obter o Id do usuário.");
        }

        // Cria um novo membro a partir do DTO e do Id do usuário
        MembrosFamilia novoMembrosFamilia = new MembrosFamilia()
        {
            NomeMembro = membroFamiliaDto.NomeMembro,
            VinculoFamilia = membroFamiliaDto.VinculoFamilia,
            FamiliaId = membroFamiliaDto.FamiliaId,
            CreatedByUserId = userId
        };

        // Salva no banco de dados
        _entidadesContext.MembrosFamilia.Add(novoMembrosFamilia);
        await _entidadesContext.SaveChangesAsync();
    }
}
