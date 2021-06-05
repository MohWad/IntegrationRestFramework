using AutoMapper;
using StudentAPI.DTOs;
using StudentAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentDTO_v1, Student_v1>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap();

            CreateMap<StudentDTO_v2, Student_v2>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Phone))
            .ReverseMap();
        }
    }
}
