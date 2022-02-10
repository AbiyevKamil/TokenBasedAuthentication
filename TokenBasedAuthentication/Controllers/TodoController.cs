using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokenBasedAuthentication.Configuration;
using TokenBasedAuthentication.Data;
using TokenBasedAuthentication.Data.Entity;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly JwtManager _jwtManager;

    public TodoController(AuthDbContext context, JwtManager jwtManager)
    {
        _context = context;
        _jwtManager = jwtManager;
    }

    [HttpGet]
    [Route("todos")]
    [AllowAnonymous]
    public async Task<IActionResult> Todos()
    {
        var todos = await _context.Todos.ToListAsync();
        return Ok(todos);
    }

    [HttpPost]
    [Route("addtodo")]
    public async Task<IActionResult> AddTodo(AddTodoModel model)
    {
        if (ModelState.IsValid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var user = _jwtManager.Authenticate(identity);
                var todo = new Todo()
                {
                    Title = model.Title,
                    Content = model.Title,
                    CreatedAt = DateTime.UtcNow,
                    AppUserId = user.Id,
                };
                await _context.Todos.AddAsync(todo);
                await _context.SaveChangesAsync();
                var todos = await _context.Todos
                    .Include(i => i.AppUser).ToListAsync();
                var todosDto = new TodosDTO()
                {
                    Todos = todos.Select(i => new TodoDTO()
                    {
                        Content = i.Content,
                        Title = i.Content,
                        Id = i.Id,
                        User = new UserDTO()
                        {
                            Email = i.AppUser.Email,
                            UserName = i.AppUser.UserName,
                        },
                        CreatedAt = i.CreatedAt
                    }).ToList(),
                };
                return Ok(todosDto);
            }
        }

        return BadRequest("Model is not valid.");
    }
}