using AutoMapper;
using StudentClassApi.DTOs;
using StudentClassApi.Models;
using StudentClassApi.Models.DTOs;

namespace StudentClassApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Class, ClassDTO>().ReverseMap();
        }
    }
}
