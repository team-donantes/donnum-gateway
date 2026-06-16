using Donnum.Gateway.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Donnum.Gateway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpClient<Donnum.Gateway.Application.Auth.IAuthTokenService, Donnum.Gateway.Infrastructure.HttpClients.UserServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:UserService"] ?? "https://localhost:7002");
        });

        return services;
    }
}
