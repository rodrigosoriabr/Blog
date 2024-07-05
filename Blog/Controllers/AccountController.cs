using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post(
        [FromServices] BlogDataContext context,
        [FromServices] EmailService emailService,
        [FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                "Welcome to the Blog!",
                $"Your password is <strong>{password}</strong>");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password // returning the pass only for test purposes
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("ACCPOSTEX400 - This email is already registered"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("ACCPOSTEX500 - Internal Server Error"));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromServices] BlogDataContext context,
        [FromServices] TokenService tokenService,
        [FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null) return StatusCode(401, new ResultViewModel<string>("User or password is invalid"));

        if (!PasswordHasher.Verify(user.PasswordHash!, model.Password))
            return StatusCode(401, new ResultViewModel<string>("User or password is invalid"));

        try
        {
            var token = tokenService.GenerateToken(user);

            return Ok(new ResultViewModel<string>(token, []));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("ACCLOGPOSTEX500 - Internal Server Error"));
        }
    }
}