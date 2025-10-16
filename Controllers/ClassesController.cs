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
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClassesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses() =>
            Ok(_mapper.Map<IEnumerable<ClassDto>>(
                await _context.Classes.Include(c => c.Students).ToListAsync()
            ));

        // POST: api/classes
        [HttpPost]
        public async Task<ActionResult<ClassDto>> CreateClass(CreateClassDto dto)
        {
            var newClass = _mapper.Map<Class>(dto);
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<ClassDto>(newClass));
        }

        // GET: api/classes/{classId}/students
        [HttpGet("{classId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsInClass(int classId) =>
            Ok(_mapper.Map<IEnumerable<StudentDto>>(
                await _context.Students.Where(s => s.ClassId == classId).ToListAsync()
            ));
    }
}
