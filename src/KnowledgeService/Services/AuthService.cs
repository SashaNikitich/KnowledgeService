using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using KnowledgeService.Interfaces;
using KnowledgeService.Models;
using KnowledgeService.Entities;
using KnowledgeService.Settings;
using KnowledgeService.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace KnowledgeService.Services;

public class AuthService : IAuthService
{
  private readonly SignInManager<User> _signInManager;
  private readonly UserManager<User> _userManager;
  private readonly ILogger<AuthService> _logger;
  private readonly IAuthorizationSettings _authSettings;

  public AuthService(
      SignInManager<User> signInManager,
      UserManager<User> userManager,
      ILogger<AuthService> logger,
      IAuthorizationSettings authSettings)
  {
    _logger = logger;
    _signInManager = signInManager;
    _userManager = userManager;
    _authSettings = authSettings;
  }

  public async Task<Result<TokenModel>> SignInAsync(SignInModel request)
  {
    var errors = new List<ResultError>();

    var user = await _userManager.FindByNameAsync(request.UserName);

    if (user is null)
    {
      _logger.LogInformation($"Unknown username: {request.UserName}");
      ResultError.AddError(errors, Status.UserNotFound);
      return Result.Fail<TokenModel>(errors);
    }

    var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

    if (signInResult.IsLockedOut)
    {
      ResultError.AddError(errors, Status.UserIsLocked);
      return Result.Fail<TokenModel>(errors);
    }

    if (!signInResult.Succeeded)
    {
      ResultError.AddError(errors, Status.SignInFailed);
      return Result.Fail<TokenModel>(errors);
    }

    var roles = await _userManager.GetRolesAsync(user);

    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.SigningKey));
    SymmetricSecurityKey encryptionKey;
    using (var sha256 = SHA256.Create())
    {
      var computeHash = sha256.ComputeHash(Encoding.Default.GetBytes(_authSettings.EncryptionKey));
      encryptionKey = new SymmetricSecurityKey(computeHash);
    }

    var tokenHandler = new JwtSecurityTokenHandler();

    var claimsIdentity = new ClaimsIdentity();
    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
    foreach (var role in roles)
    {
      claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
    }

    var encryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.Aes256KW,
        SecurityAlgorithms.Aes256CbcHmacSha512);
    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = claimsIdentity,
      Expires = DateTime.UtcNow.AddHours(_authSettings.ExpirationTimeInHours),
      SigningCredentials = signingCredentials,
      EncryptingCredentials = encryptingCredentials
    };

    var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
    return Result<TokenModel>.Ok(new TokenModel
    {
      Token = tokenHandler.WriteToken(token),
      ValidTo = token.ValidTo
    });
  }

  public async Task<Result<IdentityResult>> SignUpAsync(SignUpModel request)
  {
    var errors = new List<ResultError>();
    var user = new User
    {
      UserName = request.UserName,
      Email = request.Email == string.Empty ? null : request.Email
    };

    var result = await _userManager.CreateAsync(user, request.Password);
    if (!result.Succeeded)
    {
      ResultError.AddError(errors, Status.SignUpCreateUserFailed);
      return Result.Fail<IdentityResult>(errors, result);
    }

    result = await _userManager.AddToRoleAsync(user, Roles.User);

    if (!result.Succeeded)
    {
      ResultError.AddError(errors, Status.SignUpAddRoleToUserFailed);
      return Result.Fail<IdentityResult>(errors, result);
    }

    _logger.LogInformation($"New user registered: {user.UserName}");

    return Result.Ok<IdentityResult>(result);
  }
 }
