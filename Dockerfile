# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copia o arquivo de projeto usando o caminho da subpasta
COPY ["TaskManager.Api/TaskManager.Api.csproj", "TaskManager.Api/"]

# 2. Restaura as dependências
RUN dotnet restore "TaskManager.Api/TaskManager.Api.csproj"

# 3. Copia todo o conteúdo da pasta da API
COPY TaskManager.Api/ ./TaskManager.Api/

# 4. Compila e gera o .dll dentro da pasta /app/out
WORKDIR "/src/TaskManager.Api"
RUN dotnet publish "TaskManager.Api.csproj" -c Release -o /app/out

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copia o .dll gerado no estágio anterior
COPY --from=build /app/out .

# Configuração de porta para o Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# O Docker vai procurar o .dll aqui dentro do container
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]