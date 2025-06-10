using WebApp.API.Models;
using WebApp.API.Services;
using WebApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public void GetById_ReturnsCorrectUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Get")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.Add(new User { Id = 1, Username = "test", Email = "test@example.com" });
        context.SaveChanges();

        var service = new UserService(context);

        var result = service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
    }

    [Fact]
    public void Create_AddsNewUserToDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Create")
            .Options;

        using var context = new AppDbContext(options);
        var service = new UserService(context);

        var newUser = new User { Username = "newuser", Email = "new@example.com" };

        service.Create(newUser);

        var savedUser = context.Users.FirstOrDefault(u => u.Username == "newuser");
        Assert.NotNull(savedUser);
        Assert.Equal("new@example.com", savedUser?.Email);
    }

    [Fact]
    public void Delete_RemovesUserFromDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Delete")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.Add(new User { Id = 1, Username = "todelete", Email = "del@example.com" });
        context.SaveChanges();

        var service = new UserService(context);

        service.Delete(1);

        var deleted = context.Users.Find(1);
        Assert.Null(deleted);
    }

    [Fact]
    public void GetAll_ReturnsAllUsers()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_All")
            .Options;

        using var context = new AppDbContext(options);
        context.Users.AddRange(
            new User { Username = "user1", Email = "u1@example.com" },
            new User { Username = "user2", Email = "u2@example.com" }
        );
        context.SaveChanges();

        var service = new UserService(context);

        var allUsers = service.GetAll().ToList();

        Assert.Equal(2, allUsers.Count);
    }
}
