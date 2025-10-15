using System.Collections.Generic;

namespace StudentClassApi.Dtos
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StudentDto> Students { get; set; }
    }
}
