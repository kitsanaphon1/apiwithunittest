using Microsoft.AspNetCore.Mvc;
using WebApp.API.Models;
using WebApp.API.Services;

namespace WebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) => _userService = userService;

    [HttpGet]
    public IActionResult GetAll() => Ok(_userService.GetAll());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _userService.GetById(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        _userService.Create(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] User user)
    {
        if (id != user.Id) return BadRequest();
        _userService.Update(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return NoContent();
    }
}
