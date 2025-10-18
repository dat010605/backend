using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentClassApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Cấu hình Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StudentClassApi",
        Version = "v1",
        Description = "API quản lý học sinh và lớp học (Student-Class Management API)"
    });
});

// ✅ Thêm DbContext (sử dụng database trong bộ nhớ)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("StudentClassDb"));

// ✅ Thêm AutoMapper (nếu bạn có MappingProfile.cs)
builder.Services.AddAutoMapper(typeof(Program));

// Build app
var app = builder.Build();

// ✅ Hiển thị Swagger ở cả môi trường Development và Production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentClassApi v1");
    c.RoutePrefix = string.Empty; // Truy cập Swagger ngay tại http://localhost:5000
});

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run
app.Run();
