using System;
using System.ComponentModel.DataAnnotations;

namespace StudentClassApi.Dtos
{
    public class CreateStudentDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int ClassId { get; set; }
    }
}
