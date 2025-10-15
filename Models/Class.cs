using System.Collections.Generic;

namespace StudentClassApi.Models
{
    public class Class
    {
        public int Id { get; set; }                 
        public string Name { get; set; }

        // ✅ Thêm dấu ? để Students không bắt buộc
        public ICollection<Student>? Students { get; set; }
    }
}
