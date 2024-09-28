using System.Security.Cryptography;
using System.Text;

using KnowledgeService.Settings;
using KnowledgeService.Interfaces;
using KnowledgeService.Services;
using KnowledgeService.Contexts;
using KnowledgeService.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KnowledgeService.Extensions;

public static class ConfigureExtension
{
  // Sets JWT configuration.
  public static void AddJwtConfigure(this IServiceCollection services, IConfiguration cfg)
  {
    // services.Configure<AuthorizationSettings>(cfg.GetSection(nameof(AuthorizationSettings)));

    var settings = cfg.GetRequiredSection(nameof(AuthorizationSettings)).Get<AuthorizationSettings>();

    services.AddSingleton<IAuthorizationSettings, AuthorizationSettings>(_ => settings ?? throw new InvalidOperationException());

    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SigningKey));

    SymmetricSecurityKey decryptionKey;
    using (var sha256 = SHA256.Create())
    {
      var computeHash = sha256.ComputeHash(Encoding.Default.GetBytes(settings.EncryptionKey));
      decryptionKey = new SymmetricSecurityKey(computeHash);
    }

    services.AddAuthentication(opt =>
    {
      opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
      opt.RequireHttpsMetadata = false;
      opt.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = settings.Issuer,

        ValidateAudience = true,
        ValidAudience = settings.Audience,

        TokenDecryptionKey = decryptionKey,

        ValidateLifetime = true,

        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true,
      };
    });
  }

  public static void AddAppServices(this IServiceCollection services)
  {
    services.AddScoped<IAuthService, AuthService>();
  }

  public static void AddDbConfigure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddDbContext<ApplicationDbContext>(options =>
    {
      options.UseLazyLoadingProxies()
        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddIdentity<User, IdentityRole>(opts =>
    {
      opts.Password.RequiredLength = 5;
      opts.Password.RequireNonAlphanumeric = false;
      opts.Password.RequireLowercase = false;
      opts.Password.RequireUppercase = false;
      opts.Password.RequireDigit = false;
      opts.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
  }

  private static async Task InitAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
  {
    const string userName = "admin";
    const string email = "admin@gmail.com";
    const string password = "password";

    if (await roleManager.FindByNameAsync(Roles.Admin) == null)
    {
      await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }

    if (await roleManager.FindByNameAsync(Roles.User) == null)
    {
      await roleManager.CreateAsync(new IdentityRole(Roles.User));
    }

    if (await userManager.FindByEmailAsync(email) == null)
    {
      User admin = new User
      {
        Email = email,
        UserName = userName,
      };
      IdentityResult result = await userManager.CreateAsync(admin, password);

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(admin, Roles.Admin);
      }
    }
  }

  public static async Task HostRunAsync(this IHost host)
  {
    using (var scope = host.Services.CreateScope())
    {
      var services = scope.ServiceProvider;

      var userManager = services.GetRequiredService<UserManager<User>>();
      var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
      await InitAsync(userManager, rolesManager);
    }
    await host.RunAsync();
  }
}
