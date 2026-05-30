# CODING GUIDELINES - Donnum Microservices

Esta guia define las convenciones estrictas de codigo para todo el equipo en C# 14 y .NET 10. Todo el codigo subido en Pull Requests debe adherirse a estas reglas.

## 1. Primary Constructors para Inyeccion de Dependencias

Utiliza siempre Primary Constructors en lugar de declarar campos privados y constructores explicitos.

**Incorrecto:**
```csharp
public class PingCommandHandler : IRequestHandler<PingCommand, string>
{
    private readonly ILogger<PingCommandHandler> _logger;

    public PingCommandHandler(ILogger<PingCommandHandler> logger)
    {
        _logger = logger;
    }
}
```

**Correcto:**
```csharp
public class PingCommandHandler(ILogger<PingCommandHandler> logger) : IRequestHandler<PingCommand, string>
{
    public async Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing ping...");
        return await Task.FromResult("Pong");
    }
}
```

## 2. Records para DTOs, Comandos y Queries

Todos los objetos de transferencia de datos deben ser inmutables. Utiliza `record` en lugar de `class`.

**Incorrecto:**
```csharp
public class CreateUrgencyCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
}
```

**Correcto:**
```csharp
public record CreateUrgencyCommand(string Title, string Description) : IRequest<Guid>;
```

## 3. Collection Expressions

Para inicializar listas, arreglos o cualquier coleccion, utiliza la sintaxis de expresiones de coleccion (`[]`).

**Incorrecto:**
```csharp
var ids = new List<int> { 1, 2, 3 };
var arr = new int[] { 4, 5, 6 };
```

**Correcto:**
```csharp
List<int> ids = [1, 2, 3];
int[] arr = [4, 5, 6];
```

## 4. Uso Obligatorio y Propagacion de CancellationToken

Todo metodo asincrono debe recibir un `CancellationToken` y debe ser propagado a todas las llamadas asincronas subsecuentes, especialmente a las consultas de Entity Framework Core o llamadas HTTP.

**Incorrecto:**
```csharp
public async Task<User> GetUserAsync(Guid id)
{
    return await dbContext.Users.FindAsync(id);
}
```

**Correcto:**
```csharp
public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken)
{
    return await dbContext.Users.FindAsync([id], cancellationToken);
}
```
