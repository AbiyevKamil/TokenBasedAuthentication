using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TokenBasedAuthentication.Configuration;
using TokenBasedAuthentication.Data;
using TokenBasedAuthentication.Data.Entity;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtManager _jwtManager;

    public AuthController(AuthDbContext context, UserManager<AppUser> userManager, JwtManager jwtManager)
    {
        _context = context;
        _userManager = userManager;
        _jwtManager = jwtManager;
    }

    // GET
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Username,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = _jwtManager.GenerateToken(user);
                return Ok(new AuthResult()
                {
                    Succeeded = true,
                    Token = token,
                });
            }
            
            var managerErrResult = new AuthResult()
            {
                Errors = result.Errors.Select(i => i.Description).ToList()
            };
            return BadRequest(managerErrResult);
        }

        var errors = new List<string>();
        ModelState.Values.Aggregate(errors, (a, c) =>
        {
            a.AddRange(c.Errors.Select(e => e.ErrorMessage));
            return a;
        });
        var badReqResult = new AuthResult()
        {
            Errors = errors
        };
        return BadRequest(badReqResult);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var exist = await _userManager.CheckPasswordAsync(user, model.Password);
                if (exist)
                {
                    var token = _jwtManager.GenerateToken(user);
                    return Ok(new AuthResult()
                    {
                        Token = token,
                        Succeeded = true,
                    });
                }

                return NotFound(new AuthResult()
                {
                    Errors = new List<string>() {"Password is wrong."}
                });
            }

            return NotFound(new AuthResult()
            {
                Errors = new List<string>() {"Email is not registered."}
            });
        }

        var errors = new List<string>();
        ModelState.Values.Aggregate(errors, (a, c) =>
        {
            a.AddRange(c.Errors.Select(e => e.ErrorMessage));
            return a;
        });
        var badReqResult = new AuthResult()
        {
            Errors = errors
        };
        return BadRequest(badReqResult);
    }
}