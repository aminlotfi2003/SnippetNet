namespace SnippetNet.Infrastructure.Persistence.Seed;

public class SeedOptions
{
    public AdminUserOptions Admin { get; set; } = new();
    public TestUserOptions? TestContributor { get; set; }

    public class AdminUserOptions
    {
        public string Email { get; set; } = "admin@local";
        public string Password { get; set; } = "Pass123$";
    }

    public class TestUserOptions
    {
        public string Email { get; set; } = "contrib@local";
        public string Password { get; set; } = "Pass123$";
        public string[] Roles { get; set; } = new[] { "Contributor" };
    }
}
