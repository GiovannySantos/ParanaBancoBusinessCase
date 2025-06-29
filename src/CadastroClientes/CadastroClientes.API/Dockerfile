FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CadastroClientes/CadastroClientes.API/CadastroClientes.API.csproj", "CadastroClientes/CadastroClientes.API/"]
COPY ["CadastroClientes/CadastroClientes.Application/CadastroClientes.Application.csproj", "CadastroClientes/CadastroClientes.Application/"]
COPY ["CadastroClientes/CadastroClientes.Infra/CadastroClientes.Infra.csproj", "CadastroClientes/CadastroClientes.Infra/"]

RUN dotnet restore "CadastroClientes/CadastroClientes.API/CadastroClientes.API.csproj"

# Copia só o necessário após restore
COPY . .

WORKDIR "/src/CadastroClientes/CadastroClientes.API"
RUN dotnet publish "CadastroClientes.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CadastroClientes.API.dll"]
