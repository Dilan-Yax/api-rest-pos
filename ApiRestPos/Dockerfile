# Dockerfile para API REST POS (ASP.NET Core Web API)
# Compatible con Visual Studio 2026 y contenedores Docker

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiRestPos/ApiRestPos.csproj", "ApiRestPos/"]
RUN dotnet restore "ApiRestPos/ApiRestPos.csproj"
COPY . .
WORKDIR "/src/ApiRestPos"
RUN dotnet build "ApiRestPos.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApiRestPos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiRestPos.dll"]
