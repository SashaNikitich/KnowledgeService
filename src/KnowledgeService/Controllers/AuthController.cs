using Microsoft.AspNetCore.Mvc;

namespace KnowledgeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    public ActionResult Test()
    {
        return Ok("Test");
    }

    [HttpPost("SignIn")]
    public ActionResult SignIn()
    {
        return Ok("Test");
    }
}
