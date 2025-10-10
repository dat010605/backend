using AutoMapper;
using StudentClassApi.Models;
using StudentClassApi.Dtos;

namespace StudentClassApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));
        }
    }
}
