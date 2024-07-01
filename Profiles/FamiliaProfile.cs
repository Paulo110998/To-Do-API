using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;


public class FamiliaProfile : Profile
{
    public FamiliaProfile()
    {
        CreateMap<CreateFamiliaDto, Familia>();

        CreateMap<Familia, ReadFamiliaDto>()
            .ForMember(familiaDto => familiaDto.ReadMembrosFamilias,
            opt => opt.MapFrom(familia => familia.membrosFamilias));

        CreateMap<UpdateFamiliaDto, Familia>();
    }
}
