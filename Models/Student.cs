using System;

namespace StudentClassApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        // Foreign key
        public int ClassId { get; set; }
        // Navigation property
        public Class? Class { get; set; }
    }
}