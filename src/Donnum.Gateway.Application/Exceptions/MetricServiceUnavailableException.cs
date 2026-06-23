using System;

namespace Donnum.Gateway.Application.Exceptions;

public class MetricServiceUnavailableException : Exception
{
    public MetricServiceUnavailableException(string message) 
        : base(message)
    {
    }

    public MetricServiceUnavailableException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
