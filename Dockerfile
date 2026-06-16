FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Donnum.Gateway.Presentation.API/Donnum.Gateway.Presentation.API.csproj", "src/Donnum.Gateway.Presentation.API/"]
COPY ["src/Donnum.Gateway.Application/Donnum.Gateway.Application.csproj", "src/Donnum.Gateway.Application/"]
COPY ["src/Donnum.Gateway.Domain/Donnum.Gateway.Domain.csproj", "src/Donnum.Gateway.Domain/"]
COPY ["src/Donnum.Gateway.Infrastructure/Donnum.Gateway.Infrastructure.csproj", "src/Donnum.Gateway.Infrastructure/"]

RUN dotnet restore "src/Donnum.Gateway.Presentation.API/Donnum.Gateway.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/Donnum.Gateway.Presentation.API"
RUN dotnet build "Donnum.Gateway.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Donnum.Gateway.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Donnum.Gateway.Presentation.API.dll"]
