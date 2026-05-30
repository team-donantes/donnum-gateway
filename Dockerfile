FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Donnum.Microservice.Presentation.API/Donnum.Microservice.Presentation.API.csproj", "src/Donnum.Microservice.Presentation.API/"]
COPY ["src/Donnum.Microservice.Application/Donnum.Microservice.Application.csproj", "src/Donnum.Microservice.Application/"]
COPY ["src/Donnum.Microservice.Domain/Donnum.Microservice.Domain.csproj", "src/Donnum.Microservice.Domain/"]
COPY ["src/Donnum.Microservice.Infrastructure/Donnum.Microservice.Infrastructure.csproj", "src/Donnum.Microservice.Infrastructure/"]

RUN dotnet restore "src/Donnum.Microservice.Presentation.API/Donnum.Microservice.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/Donnum.Microservice.Presentation.API"
RUN dotnet build "Donnum.Microservice.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Donnum.Microservice.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Donnum.Microservice.Presentation.API.dll"]
