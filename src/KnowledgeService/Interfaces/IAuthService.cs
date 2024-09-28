using Microsoft.AspNetCore.Identity;
using KnowledgeService.Models;

namespace KnowledgeService.Interfaces;

public interface IAuthService
{
  Task<Result<TokenModel>> SignInAsync(SignInModel request);
  Task<Result<IdentityError>> SignUpAsync(SignUpModel request);
}
