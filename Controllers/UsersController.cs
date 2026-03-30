using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_userService.GetAll());
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        _userService.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
    
    [Authorize]
    [HttpPut]
    public IActionResult Put(int id, [FromBody] UpdateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };
        _userService.Update(id, user);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok();
    }
}