using AutoMapper;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserDetailsForTeamViewModel>();
        }
    }
}
