using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class GeradorSenhaProfile : Profile
{
    public GeradorSenhaProfile()
    {
        CreateMap<CreateGeradorSenhasDto, GeradorSenhas>();

        CreateMap<GeradorSenhas, ReadGeradorSenhaDto>();
    }
}
