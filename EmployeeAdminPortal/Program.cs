using EmployeeAdminPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   
              .AllowAnyMethod()   
              .AllowAnyHeader();  
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!builder.Environment.IsDevelopment())
{
    var envCon = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrEmpty(envCon))
    {
        connectionString = envCon; 
    }
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString) // Đã sửa lỗi cú pháp ở đây
);

var app = builder.Build();

// 4. KÍCH HOẠT CORS (Bắt buộc phải nằm ở vị trí này)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();