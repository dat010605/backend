using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClassApi.Data;
using StudentClassApi.Dtos;
using StudentClassApi.Models;
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

        // ✅ GET: api/Students (có phân trang)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents(int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.Students.Include(s => s.Class).AsQueryable();

            var totalItems = await query.CountAsync();

            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);

            var result = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                totalItems,
                pageSize,
                students = studentDtos
            };

            return Ok(result);
        }

        // ✅ GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            return Ok(_mapper.Map<StudentDto>(student));
        }

        // ✅ POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto dto)
        {
            var studentClass = await _context.Classes.FindAsync(dto.ClassId);
            if (studentClass == null)
                return BadRequest("Class not found");

            var student = new Student
            {
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth,
                ClassId = dto.ClassId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        // ✅ PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            student.Name = dto.Name;
            student.DateOfBirth = dto.DateOfBirth;
            student.ClassId = dto.ClassId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
