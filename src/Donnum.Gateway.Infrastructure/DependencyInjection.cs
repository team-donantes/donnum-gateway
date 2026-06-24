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

        services.AddHttpClient<Donnum.Gateway.Application.Contracts.IDonorServiceClient, Donnum.Gateway.Infrastructure.HttpClients.DonorServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:DonorService"] ?? "https://localhost:7001");
        });

        services.AddHttpClient<Donnum.Gateway.Application.Contracts.IBloodRequestServiceClient, Donnum.Gateway.Infrastructure.HttpClients.BloodRequestServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:BloodRequestService"] ?? "https://localhost:8083");
        });

        return services;
    }
}
