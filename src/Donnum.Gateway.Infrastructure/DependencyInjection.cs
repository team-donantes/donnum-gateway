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
            client.BaseAddress = new Uri(configuration["ServiceUrls:UserService"] ?? "http://localhost:8081");
        });

        services.AddHttpClient<Donnum.Gateway.Application.Contracts.IDonorServiceClient, Donnum.Gateway.Infrastructure.HttpClients.DonorServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:DonorService"] ?? "http://localhost:8082");
        });

        services.AddHttpClient<Donnum.Gateway.Application.Contracts.IMetricServiceClient, Donnum.Gateway.Infrastructure.HttpClients.MetricServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:MetricService"] ?? "http://localhost:8085");
        });

        return services;
    }
}
