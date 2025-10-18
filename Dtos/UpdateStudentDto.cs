using System;

namespace StudentClassApi.Dtos
{
    public class UpdateStudentDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ClassId { get; set; }
    }
}
