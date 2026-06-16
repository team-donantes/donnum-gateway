using Donnum.Gateway.Application.Auth;
using Microsoft.AspNetCore.Http;

namespace Donnum.Gateway.Presentation.API.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthTokenService authTokenService)
    {
        // Example logic to extract token and validate
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            
            var isValid = await authTokenService.ValidateTokenAsync(token, context.RequestAborted);
            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid Token");
                return;
            }
        }
        else
        {
            // If the route requires authentication, we could return 401 here.
            // For now, we'll let it pass and assume downstream or specific policies handle it,
            // or we can enforce it here depending on the Gateway's exact responsibilities.
        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}
