using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento,EventoDTO>()
            .ForMember(dest => dest.Palestrantes, opt => {
                opt.MapFrom(src => src.PalestrantesEventos.Select(x => x.Palestrante).ToList());
            }).ReverseMap();

            CreateMap<Lote,LoteDTO>().ReverseMap();

            CreateMap<Palestrante,PalestranteDTO>()
            .ForMember(dest => dest.Eventos, opt => {
                opt.MapFrom(src => src.PalestrantesEventos.Select(x => x.Evento).ToList());
            }).ReverseMap();

            CreateMap<RedeSocial,RedeSocialDTO>().ReverseMap();
            CreateMap<User,UserDTO>().ReverseMap();
            CreateMap<User,UserLoginDTO>().ReverseMap();
        }
    }
}