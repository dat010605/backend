using System.Collections.Generic;

namespace StudentClassApi.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Navigation property
        public ICollection<Student>? Students { get; set; }
    }
}