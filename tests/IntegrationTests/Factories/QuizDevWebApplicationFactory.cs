using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizDev.Infrastructure.Data;

namespace IntegrationTests.Factories;

public class QuizDevWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Tests");

        builder.ConfigureServices(services =>
        {
            // Remove a configuração do DbContext real
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<QuizDevDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<QuizDevDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });
        });
    }
}
