# Usa a imagem oficial do SDK do .NET 8 para compilar o código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia a pasta do projeto da API
COPY TaskManager.Api/ ./TaskManager.Api/

# Restaura as dependências (baseado no seu .csproj)
RUN dotnet restore "TaskManager.Api/TaskManager.Api.csproj"

# Publica a aplicação
RUN dotnet publish "TaskManager.Api/TaskManager.Api.csproj" -c Release -o out

# Usa a imagem de runtime mais leve para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta que o Render vai usar
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]