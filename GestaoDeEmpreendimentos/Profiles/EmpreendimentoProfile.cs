using AutoMapper;
using GestaoDeEmpreendimentos.Dtos;
using GestaoDeEmpreendimentos.Models;

namespace GestaoDeEmpreendimentos.Profiles
{
    // Perfil do AutoMapper para mapear entre Empreendimento (entidade) e DTOs
    public class EmpreendimentoProfile : Profile
    {
        // Construtor: define as configurações de mapeamento
        public EmpreendimentoProfile()
        {
            CreateMap<EmpreendimentoCreateDto, Empreendimento>();
            CreateMap<EmpreendimentoUpdateDto, Empreendimento>();
            CreateMap<Empreendimento, EmpreendimentoReadDto>();
        }
    }
}
