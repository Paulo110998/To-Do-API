using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class MembrosFamiliaProfile : Profile
{
    public MembrosFamiliaProfile()
    {
        CreateMap<CreateMembroFamiliaDto, MembrosFamilia>();

        CreateMap<MembrosFamilia, ReadMembroFamiliaDto>();

        CreateMap<UpdateMembroFamiliaDto, MembrosFamilia>();
    }
}
