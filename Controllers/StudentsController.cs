using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClassApi.Data;
using StudentClassApi.Models.DTOs;   // cần cho StudentDTO, ClassDTO
using AutoMapper;
using StudentClassApi.Models;             // cần cho AutoMapper

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

        //  GET: api/students?pageNumber=1&pageSize=10&search=nguyen
        [HttpGet]
        public async Task<ActionResult<PaginationResult<StudentDTO>>> GetStudents(
            int pageNumber = 1, int pageSize = 10, string? search = null)
        {
            var query = _context.Students
                .Include(s => s.Class)
                .AsQueryable();

            // Nếu có từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(search) ||
                    (s.Class != null && s.Class.Name.ToLower().Contains(search))
                );
            }

            // Đếm tổng số bản ghi (sau khi lọc)
            var totalCount = await query.CountAsync();

            // Phân trang
            var students = await query
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map sang DTO
            var studentDtos = _mapper.Map<List<StudentDTO>>(students);

            // Gói vào PaginationResult
            var result = new PaginationResult<StudentDTO>(
                studentDtos, totalCount, pageSize, pageNumber
            );

            return Ok(result);
        }

         // GET: api/classes/{classId}/students
        [HttpGet("/api/classes/{classId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsByClass(int classId)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Include(s => s.Class)
                .ToListAsync();

            if (!students.Any())
                return NotFound($"Không có sinh viên nào trong lớp {classId}");

            var studentDtos = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDtos);
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> CreateStudent(StudentDTO studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            var createdStudentDto = _mapper.Map<StudentDTO>(student);
            return CreatedAtAction(nameof(GetStudents), new { id = createdStudentDto.Id }, createdStudentDto);
        }
    }
}