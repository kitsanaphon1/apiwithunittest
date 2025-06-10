using WebApp.API.Data;
using WebApp.API.Services;
using WebApp.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Service registrations
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS สำหรับ frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ อ่าน connection string จาก Environment Variable หรือ fallback ที่ appsettings
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"📦 Using connection string: {connectionString}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// ✅ Middleware
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ✅ Migration + Seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();

        if (!db.Users.Any())
        {
            var users = Enumerable.Range(1, 50).Select(i => new User
            {
                Username = $"user{i}",
                Email = $"user{i}@example.com"
            }).ToList();

            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
    }
}

app.Run();
