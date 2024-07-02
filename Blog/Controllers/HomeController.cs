using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    // ApiKey is a way to give access to your Api without running by the login process
    // Be careful with this approach as the ApiKey is static and might be harmful
    [ApiKey]
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok();
    }
}