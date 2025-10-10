using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClassApi.Data;
using StudentClassApi.Models;

namespace StudentClassApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return await _context.Classes.Include(c => c.Students).ToListAsync();
        }

        // POST: /api/classes
        [HttpPost]
        public async Task<ActionResult<Class>> CreateClass(Class cls)
        {
            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClasses), new { id = cls.Id }, cls);
        }
    }
}
