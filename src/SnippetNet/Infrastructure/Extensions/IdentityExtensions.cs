using Microsoft.AspNetCore.Identity;
using SnippetNet.Infrastructure.Persistence;

namespace SnippetNet.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityAndAuth(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

        services.ConfigureApplicationCookie(opt =>
        {
            opt.LoginPath = "/Identity/Account/Login";
            opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
            opt.LogoutPath = "/Identity/Account/Logout";
            opt.SlidingExpiration = true;
        });

        services.AddAuthentication().AddCookie();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanManageAll", p => p.RequireRole("Admin"));
            options.AddPolicy("CanContribute", p => p.RequireRole("Contributor", "Admin"));
        });

        return services;
    }
}
