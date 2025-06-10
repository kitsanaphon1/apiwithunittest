using WebApp.API.Data;
using WebApp.API.Services;
using WebApp.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// 🌱 เพิ่ม seed data 50 users
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Users.Any())
    {
        var users = new List<User>();
        for (int i = 1; i <= 50; i++)
        {
            users.Add(new User
            {
                Username = $"user{i}",
                Email = $"user{i}@example.com"
            });
        }

        db.Users.AddRange(users);
        db.SaveChanges();
    }
}
app.Run();
