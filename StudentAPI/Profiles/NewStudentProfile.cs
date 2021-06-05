using AutoMapper;
using StudentAPI.DTOs;
using StudentAPI.Models.v1;
using StudentAPI.Models.v2;

namespace StudentAPI.Profiles
{
    public class NewStudentProfile : Profile
    {
        public NewStudentProfile()
        {
            CreateMap<NewStudent_v1, StudentDTO_v1>()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FamilyName));

            CreateMap<NewStudent_v2, StudentDTO_v2>()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FamilyName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Mobile));
        }
    }
}
