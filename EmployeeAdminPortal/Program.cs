using EmployeeAdminPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CẤU HÌNH CORS (Bắt buộc để React/Angular/Web khác gọi được API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Cho phép mọi domain
              .AllowAnyMethod()   // Cho phép mọi phương thức (GET, POST, PUT, DELETE...)
              .AllowAnyHeader();  // Cho phép mọi loại header
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. CẤU HÌNH CHUỖI KẾT NỐI DATABASE (Local & Railway)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!builder.Environment.IsDevelopment())
{
    var envCon = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrEmpty(envCon))
    {
        // Đoạn code "Phép thuật" giúp tự động dịch link URL của Railway sang chuẩn của .NET
        var databaseUri = new Uri(envCon);
        var userInfo = databaseUri.UserInfo.Split(':');

        connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    }
}

// 3. KÍCH HOẠT ENTITY FRAMEWORK VỚI POSTGRESQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

var app = builder.Build();

// 4. KÍCH HOẠT CORS (Bắt buộc phải nằm ở vị trí này, trước UseAuthorization)
app.UseCors("AllowAll");


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();