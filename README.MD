# 📋 Sistema de Gerenciamento de Tarefas - Task Manager

> Um sistema completo para gerenciamento de tarefas, construído com .NET 10 e React 19.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![React](https://img.shields.io/badge/React-19.2-61DAFB?style=flat-square&logo=react)
![Tailwind CSS](https://img.shields.io/badge/Tailwind-4.1-06B6D4?style=flat-square&logo=tailwindcss)

## ✨ Características

- 🎯 **CRUD Completo** - Criação, leitura, atualização e exclusão de tarefas
- 🔍 **Filtros Avançados** - Filtre por status, data de criação e outras maneiras
- 📊 **Ordenação Flexível** - Organize suas tarefas por título, status ou data
- 🎨 **Interface Moderna** - UI responsiva e intuitiva com Tailwind CSS
- ⚡ **Performance** - API RESTful rápida e eficiente
- ✅ **Testes Automatizados** - Cobertura de testes unitários
- 📱 **Responsivo** - Funciona perfeitamente em outros dispositivos

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

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado em sua máquina:

### Obrigatório
- **[.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)** ou superior
- **[Node.js 20.19+](https://nodejs.org/)** ou superior

### Recomendado
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** ou **[Visual Studio Code](https://code.visualstudio.com/)**
- **[Git](https://git-scm.com/)**

### Verificando as instalações

```bash
# Verificar .NET
dotnet --version
# Esperado: 10.0.x ou superior

# Verificar Node.js
node --version
# Esperado: v20.19.x ou superior
```

## 🚀 Instalação e Configuração

### 1️⃣ Clone o Repositório

```bash
git clone https://github.com/gurufiredark/task-manager-system.git
cd task-manager-system
```

### 2️⃣ Configuração do Backend (.NET API)

```bash
# Navegar para o diretório da API
cd TaskManager.Api

# Restaurar dependências do .NET
dotnet restore

# (Opcional) Verificar se tudo está OK
dotnet build
```

#### Estrutura do Backend
```
TaskManager.Api/
├── Controllers/          # Controladores da API
├── DTOs/                # Data Transfer Objects
├── Entities/            # Modelos de domínio
├── Services/            # Lógica de negócio
├── Repositories/        # Camada de dados
├── Validation/          # Validações customizadas
└── Data/               # Armazenamento JSON
    └── tasks.json      # Banco de dados (criado automaticamente)
```

### 3️⃣ Configuração do Frontend (React)

```bash
# Voltar para a raiz do projeto
cd ..

# Navegar para o diretório do frontend
cd task-manager-frontend

# Instalar dependências do Node.js
npm install
```

#### Estrutura do Frontend
```
task-manager-frontend/
├── src/
│   ├── components/      # Componentes React
│   │   ├── FilterBar.jsx
│   │   ├── TaskForm.jsx
│   │   └── TaskItem.jsx
│   ├── services/        # Serviços (API)
│   │   └── api.js
│   ├── App.jsx         # Componente principal
│   ├── main.jsx        # Entry point
│   └── index.css       # Estilos globais
├── public/             # Arquivos estáticos
└── package.json        # Dependências do Node
```

## ▶️ Executando a Aplicação

### Tenha 2 Terminais aberto
- **Recomendo** - Iniciar o Backend primeiro para que o Swagger esteja disponível e o banco de dados JSON seja inicializado antes do Frontend tentar realizar a primeira requisição.

#### Terminal 1 - Backend (API)
```bash
cd TaskManager.Api
dotnet run
```

A API estará disponível em:
- 🌐 **Swagger UI**: http://localhost:5239

#### Terminal 2 - Frontend (React)
```bash
cd task-manager-frontend
npm run dev
```

O frontend estará disponível em:
- 🎨 **Aplicação**: http://localhost:5173


## 🧪 Executando os Testes

### Testes do Backend

```bash
# Navegar para o diretório de testes
cd TaskManager.Tests

# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --logger "console;verbosity=detailed"

# Executar com cobertura de código
dotnet test /p:CollectCoverage=true
```

**Testes disponíveis:**
- ✅ Testes de Repository (JsonTaskRepository)
- ✅ Testes de Service (TaskService)
- ✅ Testes de filtros e ordenação
- ✅ Testes de validação

### Estrutura dos Testes
```
TaskManager.Tests/
├── Repositories/
│   └── JsonTaskRepositoryTests.cs
└── Services/
    └── TaskServiceTests.cs
```

## 📖 Documentação da API

### Swagger UI
Acesse http://localhost:5239 após iniciar o backend para visualizar a documentação interativa completa da API.

**Query Parameters:**
- `status` (int, opcional): 0 (Pending), 1 (InProgress), 2 (Completed)
- `orderBy` (string, opcional): title, status, createdAt, updatedAt
- `orderDirection` (string, opcional): asc, desc
- `createdAfter` (datetime, opcional): Filtrar por data de criação
- `createdBefore` (datetime, opcional): Filtrar por data de criação

### Status da Tarefa
- `0` - **Pending** (Pendente)
- `1` - **InProgress** (Em Progresso)
- `2` - **Completed** (Concluída)

## 🐛 Solução de Problemas

### 🌐 Conexão Frontend -> API
Se houver erro de conexão (`Network Error`):
1. Confirme se o backend está rodando em `http://localhost:5239`.
2. Verifique a URL base no arquivo `task-manager-frontend/src/services/api.js`.

### 🚪 Porta 5239 ou 5173 ocupada
Se a porta já estiver em uso, você pode encerrar o processo:
- **Windows:** `netstat -ano | findstr :PORTA` -> `taskkill /PID [PID] /F`
- **Linux/Mac:** `lsof -ti:PORTA | xargs kill -9`

### 📦 Erro de Dependências
Caso ocorra algum erro na instalação, tente limpar o cache:
- **Backend:** `dotnet clean` e `dotnet restore`
- **Frontend:** Remova a pasta `node_modules` e execute `npm install` novamente.

## 📁 Estrutura de Dados

O arquivo `tasks.json` é criado automaticamente em `TaskManager.Api/Data/`:

```json
[
  {
    "Id": "72c959fd-050c-492b-bb26-4ff4c6339d04",
    "Title": "Estudar React",
    "Description": "Aprender hooks e context API",
    "Status": 0,
    "CreatedAt": "2026-02-02T11:28:36.495Z",
    "UpdatedAt": null
  }
]
```

**⚠️ Importante:**
- O arquivo é criado automaticamente na primeira execução
- Não é necessário criar manualmente


**Hospedagem:**
- **Backend**: Railway
- **Frontend**: Vercel

## 👨‍💻 Autor

**Gabriel Rodrigues de Souza**

- GitHub: [@gurufiredark](https://github.com/gurufiredark)
- LinkedIn: [gabrielrodriguesguru](https://linkedin.com/in/gabrielrodriguesguru)