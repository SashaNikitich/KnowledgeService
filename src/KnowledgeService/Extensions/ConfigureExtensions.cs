using KnowledgeService.Models;
using KnowledgeService.Settings;
using KnowledgeService.Contexts;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public static class ConfigureExtension
{
  // Sets JWT configuration.
  public static void AddJwtConfigure(this IServiceCollection services, IConfiguration cfg)
  {
    services.Configure<AuthorizationSettings>(cfg.GetSection(nameof(AuthorizationSettings)));

    services.AddSingleton<IAuthorizationSettings>(sp =>
        sp.GetRequiredService<IOptions<AuthorizationSettings>>().Value);

    var options = cfg.GetSection(nameof(AuthorizationSettings)).Get<AuthorizationSettings>();
  }
}
