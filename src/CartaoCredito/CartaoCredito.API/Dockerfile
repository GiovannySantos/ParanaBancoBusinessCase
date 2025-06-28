# Etapa base de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CartaoCredito/CartaoCredito.API/CartaoCredito.API.csproj", "CartaoCredito/CartaoCredito.API/"]
RUN dotnet restore "CartaoCredito/CartaoCredito.API/CartaoCredito.API.csproj"

COPY . .
WORKDIR "/src/CartaoCredito/CartaoCredito.API"
RUN dotnet build "CartaoCredito.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publish
FROM build AS publish
RUN dotnet publish "CartaoCredito.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartaoCredito.API.dll"]
