using SmartKazanlak.Infrastructure.Seed;

namespace SmartKazanlak.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<WebApplication> MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();

            await IdentitySeeder.SeedAsync(services, config);

            return app;
        }
    }
}
