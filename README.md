# 📋 Sistema de Gerenciamento de Tarefas - Task Manager

> Um sistema completo para gerenciamento de tarefas, construído em C# com .NET 10 e React.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![React](https://img.shields.io/badge/React-19.2-61DAFB?style=flat-square&logo=react)
![Tailwind CSS](https://img.shields.io/badge/Tailwind-4.1-06B6D4?style=flat-square&logo=tailwindcss)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

## 📑 Índice

- [Características](#-características)
- [Tecnologias](#️-tecnologias-utilizadas)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Rodando Localmente](#-rodando-localmente)
- [Docker (Deploy no Render)](#-docker-deploy-no-render)
- [Testes](#-testes)
- [Documentação da API](#-documentação-da-api)
- [Estrutura de Dados](#-estrutura-de-dados)
- [Deploy](#-deploy)
- [Autor](#-autor)

## ✨ Características

- 🎯 **CRUD Completo** - Criação, leitura, atualização e exclusão de tarefas
- 🔍 **Filtros Avançados** - Filtre por status, data de criação e outras maneiras
- 📊 **Ordenação Flexível** - Organize suas tarefas por título, status ou data
- 🎨 **Interface Moderna** - UI responsiva e intuitiva com Tailwind CSS
- ⚡ **Performance** - API RESTful rápida e eficiente
- ✅ **Testes Automatizados** - Cobertura de testes unitários com xUnit

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 10** - Framework principal
- **ASP.NET Core** - API RESTful
- **System.Text.Json** - Persistência em JSON
- **Swagger/OpenAPI** - Documentação automática da API
- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions para testes
- **Moq** - Mock objects para testes

### Frontend
- **React 19.2** - Biblioteca UI
- **Vite 7** - Build tool e dev server
- **Tailwind CSS 4** - Framework CSS
- **Axios** - Cliente HTTP
- **React Icons** - Biblioteca de ícones

## 📁 Estrutura do Projeto

```
task-manager-system/
├── TaskManager.Api/          # Backend - API RESTful
│   ├── Controllers/          # Controladores da API
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Entities/             # Modelos de domínio
│   ├── Services/             # Lógica de negócio
│   ├── Repositories/         # Camada de dados
│   ├── Validation/           # Validações customizadas
│   └── Data/                 # Armazenamento JSON (criado automaticamente)
│       └── tasks.json
├── task-manager-frontend/    # Frontend - React SPA
│   ├── src/
│   │   ├── components/       # Componentes React
│   │   ├── services/         # Serviço de API
│   │   └── App.jsx
│   └── package.json
├── TaskManager.Tests/        # Testes unitários
│   ├── Repositories/
│   └── Services/
├── Dockerfile                # Configuração do container (Deploy no Render)
└── TaskManager.slnx          # Arquivo de solução
```

## 📋 Pré-requisitos

### Obrigatório
- **[.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)** ou superior
- **[Node.js 20.19+](https://nodejs.org/)** ou superior

### Recomendado
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** ou **[VS Code](https://code.visualstudio.com/)**
- **[Git](https://git-scm.com/)**
- **[Docker](https://www.docker.com/)** (apenas se for fazer deploy no Render via container)

### Verificando as Instalações

```bash
# Verificar .NET
dotnet --version
# Esperado: 10.0.x ou superior

# Verificar Node.js
node --version
# Esperado: v20.19.x ou superior
```

## 🚀 Instalação

### 1. Clone o Repositório

```bash
git clone https://github.com/gurufiredark/task-manager-system.git
cd task-manager-system
```

### 2. Configuração do Backend

```bash
cd TaskManager.Api
dotnet restore
dotnet build
```

### 3. Configuração do Frontend

```bash
cd ../task-manager-frontend
npm install
```

## ▶️ Rodando Localmente

Abra **2 terminais**:

### Terminal 1 - Backend (API)

```bash
cd TaskManager.Api
dotnet run
```

A API estará disponível em:
- 🌐 **Swagger UI**: http://localhost:5239

### Terminal 2 - Frontend (React)

```bash
cd task-manager-frontend
npm run dev
```

O frontend estará disponível em:
- 🎨 **Aplicação**: http://localhost:5173

> **Dica**: Inicie o backend primeiro para que a API e o banco JSON estejam prontos antes do frontend fazer a primeira requisição.

## 🐳 Docker (Deploy no Render)

O `Dockerfile` incluído no projeto é utilizado para **empacotar o backend e fazer deploy no Render**. Ele não é necessário para rodar a aplicação localmente — use as instruções da seção [Rodando Localmente](#-rodando-localmente) para desenvolvimento.

O Docker expõe a porta `10000`, que é a porta padrão utilizada pelo Render para web services.

Se quiser testar o container localmente:

```bash
# Construir a imagem
docker build -t task-manager-api .

# Rodar o container
docker run -d -p 10000:10000 --name task-manager task-manager-api
```

A API estará disponível em: http://localhost:10000

## 🧪 Testes

```bash
cd TaskManager.Tests

# Executar todos os testes
dotnet test

# Executar com saída detalhada
dotnet test --logger "console;verbosity=detailed"

# Executar com cobertura de código
dotnet test /p:CollectCoverage=true
```

**Testes incluem:**
- ✅ Testes de Repository (`JsonTaskRepository`)
- ✅ Testes de Service (`TaskService`)
- ✅ Testes de filtros e ordenação
- ✅ Testes de validação

## 📖 Documentação da API

### Swagger UI
Acesse http://localhost:5239 após iniciar o backend para visualizar a documentação interativa completa da API.

### Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/tasks` | Listar todas as tarefas (com filtros opcionais) |
| `GET` | `/api/tasks/{id}` | Buscar tarefa por ID |
| `POST` | `/api/tasks` | Criar nova tarefa |
| `PUT` | `/api/tasks/{id}` | Atualizar tarefa existente |
| `DELETE` | `/api/tasks/{id}` | Excluir tarefa |

### Query Parameters (GET /api/tasks)

| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `status` | int | Filtrar por status: 0 (Pending), 1 (InProgress), 2 (Completed) |
| `orderBy` | string | Ordenar por: title, status, createdAt, updatedAt |
| `orderDirection` | string | Direção da ordenação: asc, desc |
| `createdAfter` | datetime | Filtrar tarefas criadas após a data |
| `createdBefore` | datetime | Filtrar tarefas criadas antes da data |

### Corpo da Requisição (POST /api/tasks)

```json
{
  "title": "Estudar .NET",
  "description": "Aprender Garbage Collector"
}
```

### Corpo da Requisição (PUT /api/tasks/{id})

```json
{
  "title": "Título atualizado",
  "description": "Descrição atualizada",
  "status": 2
}
```

### Status da Tarefa

| Valor | Status |
|-------|--------|
| `0` | Pending (Pendente) |
| `1` | InProgress (Em Progresso) |
| `2` | Completed (Concluída) |

## 📁 Estrutura de Dados

O arquivo `tasks.json` é criado automaticamente em `TaskManager.Api/Data/`:

```json
[
  {
    "Id": "72c959fd-050c-492b-bb26-4ff4c6339d04",
    "Title": "Estudar .NET",
    "Description": "Aprender Garbage Collector",
    "Status": 0,
    "CreatedAt": "2026-02-02T11:28:36.495Z",
    "UpdatedAt": null
  }
]
```

> O arquivo é criado automaticamente na primeira execução — não é necessário criar manualmente.

## 🚀 Deploy

- **Backend**: [Render](https://task-manager-system-pax0.onrender.com)
- **Frontend**: [Vercel](https://task-manager-system-omega.vercel.app/)

## 👨‍💻 Autor

**Gabriel Rodrigues de Souza**

- GitHub: [@gurufiredark](https://github.com/gurufiredark)
- LinkedIn: [gabrielrodriguesguru](https://linkedin.com/in/gabrielrodriguesguru)
