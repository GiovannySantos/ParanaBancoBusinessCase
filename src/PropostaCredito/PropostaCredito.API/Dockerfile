FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas o csproj para restaurar as dependências primeiro (melhor cache)
COPY ["PropostaCredito/PropostaCredito.API/PropostaCredito.API.csproj", "PropostaCredito/PropostaCredito.API/"]
RUN dotnet restore "PropostaCredito/PropostaCredito.API/PropostaCredito.API.csproj"

# Copia o restante dos arquivos
COPY . .

WORKDIR "/src/PropostaCredito/PropostaCredito.API"
RUN dotnet publish "PropostaCredito.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PropostaCredito.API.dll"]
