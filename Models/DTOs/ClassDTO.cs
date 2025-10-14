using System.Collections.Generic;
using StudentClassApi.Models.DTOs;
namespace StudentClassApi.DTOs
{
    public class ClassDTO
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public List<StudentDTO>? Students { get; set; }
    }
}
