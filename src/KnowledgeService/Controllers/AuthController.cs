using Microsoft.AspNetCore.Mvc;
using KnowledgeService.Models;
using KnowledgeService.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KnowledgeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly ILogger<AuthController> _logger;
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService, ILogger<AuthController> logger)
  {
    _authService = authService;
     _logger = logger;
  }

  [HttpPost("SignIn")]
  public async Task<ActionResult<Result<IdentityResult>>> SignUpAsync(SignUpModel signUpModel)
  {
    var identityResult = await _authService.SignUpAsync(signUpModel);

    return identityResult;
  }

  [HttpPost("SignIn")]
  public async Task<ActionResult<Result<TokenModel>>> SignInAsync(SignInModel signInModel)
  {
    var token = await _authService.SignInAsync(signInModel);

    return token;
  }
}
