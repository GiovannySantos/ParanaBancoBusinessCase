# Etapa base de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas o csproj para aproveitar cache
COPY ["CartaoCredito/CartaoCredito.API/CartaoCredito.API.csproj", "CartaoCredito/CartaoCredito.API/"]
RUN dotnet restore "CartaoCredito/CartaoCredito.API/CartaoCredito.API.csproj"

# Copia o restante dos arquivos
COPY . .

WORKDIR "/src/CartaoCredito/CartaoCredito.API"
RUN dotnet publish "CartaoCredito.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CartaoCredito.API.dll"]
