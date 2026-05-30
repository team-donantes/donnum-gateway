# Donnum.Microservice.Seed - Way of Work (WoW)

¡Bienvenidos al **Team Donnum**! 
Este repositorio es la plantilla (scaffolding) oficial para la construcción de los microservicios de **Donnum**, nuestra plataforma de donación de sangre en tiempo real. 

El propósito de este template es garantizar que los 5 microservicios base (Autenticación, Donantes, Urgencias, Alertas y Estadísticas) compartan la misma estructura, buenas prácticas y estándares técnicos. Así aseguramos un ecosistema unificado, escalable y mantenible por nuestros 9 desarrolladores.

---

## Arquitectura Base

Hemos adoptado **Clean Architecture**, dividida estrictamente en 4 capas respetando la Regla de Dependencia (las capas internas no conocen a las externas):

| Capa | Descripción | Dependencias |
|------|-------------|--------------|
| **Domain** | Entidades, Value Objects, y Excepciones de negocio (`DomainException`). | *Ninguna* (Es el núcleo puro). |
| **Application** | Casos de uso (CQRS), Validaciones (FluentValidation) e Interfaces. | `Domain` |
| **Infrastructure** | DbContext (EF Core SQL Server), Repositorios, Servicios Externos. | `Application` |
| **Presentation.API**| Controladores / Minimal APIs, Middleware de Errores e Inyección de Dependencias. | `Application`, `Infrastructure` |

---

## Reglas de Oro (WoW)

Para mantener la salud mental del equipo y del código, cumpliremos de forma **obligatoria** las siguientes reglas:

### 1. Persistencia Aislada
* **Cada microservicio es dueño absoluto de su base de datos** (SQL Server).
* **Prohibido:** Consultar directamente las tablas o la base de datos de otro microservicio. Si necesitas información externa, comunícate vía API, gRPC o Eventos (Message Broker).

### 2. Vertical Slicing & CQRS
* Dentro de `Core.Application`, el código se agrupa por **Funcionalidades (Features)**, no por tipo de archivo.
* **Ejemplo correcto:** `Features/CreateUrgency/` que contenga `CreateUrgencyCommand`, `CreateUrgencyValidator` y `CreateUrgencyHandler`.
* Utilizamos **MediatR** para despachar comandos y consultas.

### 3. Validaciones Estrictas
* Uso exclusivo de **FluentValidation**.
* **Prohibido:** Validar la estructura de los datos dentro de los Handlers o Controladores. Todo pasa por el `ValidationBehavior` del pipeline de MediatR. Si falla, se corta el flujo automáticamente.

### 4. Manejo de Errores Global
* **Prohibido:** El uso de bloques `try-catch` en Controladores o Handlers (a menos que sea estrictamente necesario para un retry polícy puntual).
* Si una regla de negocio se rompe, lanza un `throw new DomainException("mensaje")`.
* Nuestro `GlobalExceptionHandler` interceptará todas las excepciones (`DomainException`, `ValidationException`, etc.) y devolverá el formato estándar **RFC 7807 (Problem Details)**.

### 5. GitFlow y Reglas de Pull Requests (ESTRICTO)
* **Ramas:** Queda totalmente prohibido el push directo a las ramas `master`/`main` o `develop`. Todo desarrollo nace de `develop` en una rama con prefijo `feature/nombre-de-tarea` o `fix/nombre-del-bug`.
* **Regla de Aprobaciones:** Para que un Pull Request pueda ser mergeado, requiere obligatoriamente **más de 2 approves**.
* **Gatekeeper (Líder de Arquitectura):** Uno de esos approves obligatorios tiene que ser sí o sí del Líder de Arquitectura. Sin su visto bueno, el PR no se integra bajo ninguna circunstancia.

---

## ¿Cómo usar esta plantilla?

Este template está configurado para instalarse en el CLI de .NET. Sigue estos pasos para crear tu microservicio:

### 1. Instalar la plantilla localmente
Posiciónate en la raíz de este repositorio y ejecuta:
```bash
dotnet new install .
```

### 2. Generar un nuevo microservicio
Ve al directorio donde quieras crear tu nuevo proyecto y ejecuta el siguiente comando, reemplazando `Donnum.MicroserviceName` por el nombre real (ej. `Donnum.Autenticacion`):
```bash
dotnet new donnum-ms -n Donnum.MicroserviceName
```

¡Listo! Ya tienes una base sólida y estandarizada en .NET 10. A programar y salvar vidas con Donnum.
