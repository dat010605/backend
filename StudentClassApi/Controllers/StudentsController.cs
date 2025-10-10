using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClassApi.Data;
using StudentClassApi.Models;
using AutoMapper;
using StudentClassApi.Dtos;

namespace StudentClassApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/students?pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetStudents(int pageNumber = 1, int pageSize = 5)
        {
            var totalItems = await _context.Students.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var students = await _context.Students
                .Include(s => s.Class)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);

            return Ok(new
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = studentDtos
            });
        }

        // GET /api/classes/{classId}/students
        [HttpGet("/api/classes/{classId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByClass(int classId)
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .Where(s => s.ClassId == classId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
        }

        // POST /api/students
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            var cls = await _context.Classes.FindAsync(student.ClassId);
            if (cls == null)
                return BadRequest("Class not found");

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
        }

        // PUT /api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            var existing = await _context.Students.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Không cho đổi lớp
            existing.Name = student.Name;
            existing.DateOfBirth = student.DateOfBirth;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
