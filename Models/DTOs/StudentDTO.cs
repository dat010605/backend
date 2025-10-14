namespace StudentClassApi.Models.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int ClassId { get; set; }
    }
}