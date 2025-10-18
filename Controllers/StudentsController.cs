using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClassApi.Data;
using StudentClassApi.Models;
using StudentClassApi.Dtos;
using AutoMapper;

namespace StudentClassApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ✅ GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(studentDtos);
        }

        // ✅ GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            return Ok(_mapper.Map<StudentDto>(student));
        }

        // ✅ POST: api/students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto createStudentDto)
        {
            // Kiểm tra class có tồn tại không
            var existingClass = await _context.Classes.FindAsync(createStudentDto.ClassId);
            if (existingClass == null)
                return BadRequest($"Không tìm thấy lớp với ID = {createStudentDto.ClassId}");

            var newStudent = new Student
            {
                Name = createStudentDto.Name,
                ClassId = createStudentDto.ClassId
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            var studentDto = _mapper.Map<StudentDto>(newStudent);
            return Ok(studentDto);
        }

        // ✅ PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.Name = updateStudentDto.Name;
            student.ClassId = updateStudentDto.ClassId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
