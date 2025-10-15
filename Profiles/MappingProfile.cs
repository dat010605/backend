using AutoMapper;
using StudentClassApi.Models;
using StudentClassApi.Dtos;

namespace StudentClassApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Class, ClassDto>();
            CreateMap<Student, StudentDto>();
        }
    }
}
