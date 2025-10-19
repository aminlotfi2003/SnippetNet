using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SnippetNet.Infrastructure.Persistence.Seed;

public class IdentitySeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleMgr;
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly SeedOptions _options;
    private readonly ILogger<IdentitySeeder> _logger;

    public IdentitySeeder(
        RoleManager<IdentityRole<Guid>> roleMgr,
        UserManager<ApplicationUser> userMgr,
        IOptions<SeedOptions> options,
        ILogger<IdentitySeeder> logger)
    {
        _roleMgr = roleMgr;
        _userMgr = userMgr;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        // Roles
        var roles = new[] { "Viewer", "Contributor", "Admin" };
        foreach (var r in roles)
        {
            if (!await _roleMgr.Roles.AnyAsync(x => x.Name == r, ct))
            {
                var res = await _roleMgr.CreateAsync(new IdentityRole<Guid>(r));
                if (!res.Succeeded)
                    throw new Exception($"Failed to create role {r}: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            }
        }

        // Admin
        var admin = await _userMgr.FindByEmailAsync(_options.Admin.Email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = _options.Admin.Email,
                Email = _options.Admin.Email,
                EmailConfirmed = true
            };
            var res = await _userMgr.CreateAsync(admin, _options.Admin.Password);
            if (!res.Succeeded)
                throw new Exception($"Failed to create admin user: {string.Join(", ", res.Errors.Select(e => e.Description))}");
        }
        if (!await _userMgr.IsInRoleAsync(admin, "Admin"))
            await _userMgr.AddToRoleAsync(admin, "Admin");

        // Test Contributor
        if (_options.TestContributor is not null)
        {
            var t = await _userMgr.FindByEmailAsync(_options.TestContributor.Email);
            if (t is null)
            {
                t = new ApplicationUser
                {
                    UserName = _options.TestContributor.Email,
                    Email = _options.TestContributor.Email,
                    EmailConfirmed = true
                };
                var res = await _userMgr.CreateAsync(t, _options.TestContributor.Password);
                if (!res.Succeeded)
                    _logger.LogWarning("Failed to create test contributor: {Errors}", string.Join(", ", res.Errors.Select(e => e.Description)));
            }
            foreach (var role in _options.TestContributor.Roles)
                if (!await _userMgr.IsInRoleAsync(t!, role))
                    await _userMgr.AddToRoleAsync(t!, role);
        }
    }
}
