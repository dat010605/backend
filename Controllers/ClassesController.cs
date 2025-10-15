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
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classes = await _context.Classes.Include(c => c.Students).ToListAsync();
            var classDtos = _mapper.Map<IEnumerable<ClassDto>>(classes);
            return Ok(classDtos);
        }

        // ✅ POST: api/classes
        [HttpPost]
        public async Task<ActionResult<ClassDto>> CreateClass(CreateClassDto createClassDto)
        {
            var newClass = new Class
            {
                Name = createClassDto.Name
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            var classDto = _mapper.Map<ClassDto>(newClass);
            return Ok(classDto);
        }

        // GET: api/classes/{classId}/students
        [HttpGet("{classId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsInClass(int classId)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .ToListAsync();

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(studentDtos);
        }
    }
}
