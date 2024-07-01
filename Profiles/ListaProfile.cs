using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class ListaProfile : Profile
{
    public ListaProfile()
    {
        CreateMap<CreateListaDto, Listas>();

        CreateMap<Listas, ReadListaDto>()
            .ForMember(listaDto => listaDto.ReadTarefasNaLista,
            opt => opt.MapFrom(lista => lista.TarefasNaLista));

        CreateMap<UpdateListaDto, Listas>();
    }
}