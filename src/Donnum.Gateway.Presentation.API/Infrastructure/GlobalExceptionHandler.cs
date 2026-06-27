using Donnum.Gateway.Application.Exceptions;
using Donnum.Gateway.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Validation Error";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = "One or more validation errors occurred.";
            problemDetails.Extensions.Add("errors", validationException.Errors);
        }
        else if (exception is DomainException domainException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            problemDetails.Title = "Domain Rule Violation";
            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            problemDetails.Detail = domainException.Message;
        }
        else if (exception is MetricServiceUnavailableException metricException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            problemDetails.Title = "Metric Service Unavailable";
            problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
            problemDetails.Detail = metricException.Message;
        }
        else if (exception is NotFoundException notFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetails.Title = "Resource Not Found";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = notFoundException.Message;
        }
        else if (exception is HttpRequestException httpRequestException && httpRequestException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            problemDetails.Title = "Resource Not Found";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = "El recurso solicitado no fue encontrado en el servicio de origen.";
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "An unexpected error occurred";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = exception.Message;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
