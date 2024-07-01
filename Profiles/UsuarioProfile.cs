using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();

        CreateMap<Usuario, ReadUsuariosDto>();

        CreateMap<UpdateUsuarioDto, Usuario>();
    }
}
