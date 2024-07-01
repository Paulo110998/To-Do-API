using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class AnotacoesProfile : Profile
{
    public AnotacoesProfile()
    {
        CreateMap<CreateAnotacoesDto, Anotacoes>();

        CreateMap<Anotacoes, ReadAnotacoesDto>();

        CreateMap<UpdateAnotacoesDto, Anotacoes>();
    }
}
