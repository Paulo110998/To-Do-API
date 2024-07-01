using AutoMapper;
using TO_DO___API.Models;

namespace TO_DO___API.Profiles;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<CreateTarefaDto, Tarefas>();

        CreateMap<Tarefas, ReadTarefaDto>();

        CreateMap<UpdateTarefaDto, Tarefas>();

    }
}
